using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        private readonly Dictionary<Tuple<Size, Color>, IEnumerable<Shirt>> _shirtLookup;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            var shirtLookup = shirts.GroupBy(g => new { g.Size, g.Color });
            _shirtLookup = shirtLookup.ToDictionary(k => Tuple.Create(k.Key.Size, k.Key.Color), v => v.AsEnumerable());
        }

        public SearchResults Search(SearchOptions options)
        {
            var searchSizes = options.Sizes.Count == 0 ? Size.All : options.Sizes;
            var searchColors = options.Colors.Count == 0 ? Color.All : options.Colors;

            var results = _shirtLookup.Where(w =>
                (searchSizes.Contains(w.Key.Item1))
                &&
                (searchColors.Contains(w.Key.Item2))).SelectMany(s => s.Value).ToList();

            var sizeCount = Size.All.Select(size => new SizeCount
            {
                Size = size,
                Count = _shirtLookup.Where(k => k.Key.Item1.Id == size.Id
                    && (options.Colors.Count == 0 || options.Colors.Select(c => c.Id).Contains(k.Key.Item2.Id)))
                .SelectMany(s => s.Value)
                .Count()
            }).ToList();

            var colorCount = Color.All.Select(color => new ColorCount
            {
                Color = color,
                Count = _shirtLookup.Where(k => k.Key.Item2.Id == color.Id
                    && (options.Sizes.Count == 0 || options.Sizes.Select(c => c.Id).Contains(k.Key.Item1.Id)))
                .SelectMany(s => s.Value)
                .Count()
            }).ToList();

            return new SearchResults
            {
                Shirts = results,
                SizeCounts = sizeCount,
                ColorCounts = colorCount
            };
        }
    }
}
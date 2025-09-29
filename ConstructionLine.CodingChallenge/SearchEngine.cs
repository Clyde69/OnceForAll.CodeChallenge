using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            var searchSizes = !options.Sizes.Any() ? Size.All : options.Sizes;
            var searchColors = !options.Colors.Any() ? Color.All : options.Colors;

            var results = _shirtLookup.Where(w =>
                (searchSizes.Contains(w.Key.Item1))
                &&
                (searchColors.Contains(w.Key.Item2))).SelectMany(s => s.Value);

            var sizeCounts = Size.All.Select(s => 
                new SizeCount { Size = s, Count = results.Count(w => w.Size.Equals(s)) }).OrderBy(o => o.Size.Id);

            var colourCounts = Color.All.Select(s =>
                new ColorCount { Color = s, Count = results.Count(w => w.Color.Equals(s)) }).OrderBy(o => o.Color.Id);

            return new SearchResults
            {
                Shirts = results.ToList(),
                SizeCounts = sizeCounts.ToList(),
                ColorCounts = colourCounts.ToList()
            };
        }
    }
}
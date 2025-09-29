using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            var results = _shirts.Where(w =>
                (options.Sizes.Count == 0 || options.Sizes.Select(s => s.Id).Contains(w.Size.Id))
                &&
                (options.Colors.Count == 0 || options.Colors.Select(s => s.Id).Contains(w.Color.Id))
            ).ToList();

            return new SearchResults
            {
                Shirts = results,
                SizeCounts = Size.All.Select(size => new SizeCount
                {
                    Size = size,
                    Count = _shirts.Count(s => s.Size.Id == size.Id
                                               && (options.Colors.Count == 0 || options.Colors.Select(c => c.Id).Contains(s.Color.Id)))
                }).ToList(),
                ColorCounts = Color.All.Select(color => new ColorCount
                {
                    Color = color,
                    Count = _shirts.Count(s => s.Color.Id == color.Id
                                               && (options.Sizes.Count == 0 || options.Sizes.Select(sz => sz.Id).Contains(s.Size.Id)))
                }).ToList()
            };
        }
    }
}
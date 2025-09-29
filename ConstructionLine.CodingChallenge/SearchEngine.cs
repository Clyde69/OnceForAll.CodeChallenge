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
            // TODO: search logic goes here.
            var results = _shirts.Where(w => 
                (!options.Sizes.Any() || options.Sizes.Contains(w.Size))
                && 
                (!options.Colors.Any() || options.Colors.Contains(w.Color)));

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
//Dafny file: Promise something about the result, then Dafny proves that the promise is kept, for every output
//1.a) Implement filter mechanism using dafny function

//declaration from assignment week7


module Models{
    class {:extern} Taxon {
        function {:extern} isSubTaxon(ancestor: Taxon): bool
    }

    class {:extern} Observation {
        function {:extern} {:axiom} getTaxon(): Taxon 
    }

    function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    decreases |obs|
    {
        if |obs| == 0 then 
            [] // Base case: empty sequence
        else 
            var filteredObs:= FilterBy(root, obs[1..]); //recursively call FilterBy decreasing the number of observations with 1 each time
            if obs[0].getTaxon() == root || obs[0].getTaxon().isSubTaxon(root) //if taxon is root or subTaxon
            then [obs[0]] + filteredObs //add the observation to the filtered obs
            else filteredObs //if not just return the filteredObs
    }

    //missing ensures (specifying) clause for FilterBy function, that is the postcondition that we promise to keep, 
    //that is the output of FilterBy function is a sequence of observations that are either the root taxon or a subTaxon of the root taxon
     
    //Use (bool) predicate for the match condition, that is the taxon of the observation is either the root taxon or a subTaxon of the root taxon
    
    //mapping function to map the observations to their taxon, and then use the predicate to filter the observations based on their taxon

    //Give wrapper same promises
    /*
    module Filter
{
    import opened Models
    function FilterBy(root: Taxon, obs : seq<Observation>): seq<Observation>
}

    */
}
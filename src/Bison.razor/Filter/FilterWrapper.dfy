//Wrapper class for Filtering
//To generate C# code we can call from our main project
include "Filter.dfy"
module FilterWrapper
{
    import opened Models
    
    method getFilteredObservations(root: Taxon, obs: seq<Observation>) returns (FilteredObs: seq<Observation>){
    //do required and ensures and assert here
        FilteredObs := FilterBy(root, obs);
  }
}
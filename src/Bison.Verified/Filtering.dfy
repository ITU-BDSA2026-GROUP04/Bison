//Dafny file: Promise something about the result, then Dafny proves that the promise is kept, for every output
//1.a) Implement filter mechanism using dafny function

//declaration from assignment week7
class {:extern} Taxon{
    function {:extern} SubTaxon(ancestor: Taxon): bool
}

class {:extern} Observation{
    function {:extern} GetTaxon(): Taxon
}

function FilterBy(root: Taxon, obs : seq<Observation>): seq<Observation>
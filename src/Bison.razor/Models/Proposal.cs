namespace Models;
public class Proposal : Post
{
    public int ObservationId { get; set; }
    public Observation Observation { get; set; }

}

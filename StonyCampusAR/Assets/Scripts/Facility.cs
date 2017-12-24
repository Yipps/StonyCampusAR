[System.Serializable]
public class Facility
{
    public string name;
    public string building;
    public string department;
    public string description;
    public string organization;

    public Facility()
    {

    }

    public Facility(string name)
    {
        this.name = name;
    }

    public Facility(string name, string department)
    {
        this.name = name;
        this.department = department;
    }

    public Facility(string name, string department, string organization)
    {
        this.name = name;
        this.department = department;
        this.organization = organization;
    }
}

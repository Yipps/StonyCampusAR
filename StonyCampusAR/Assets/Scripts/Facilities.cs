[System.Serializable]
public class Facilities
{
    public string name;
    public string building;
    public string department;
    public string description;
    public string organization;

    public Facilities()
    {

    }

    public Facilities(string name)
    {
        this.name = name;
    }

    public Facilities(string name, string department)
    {
        this.name = name;
        this.department = department;
    }

    public Facilities(string name, string department, string organization)
    {
        this.name = name;
        this.department = department;
        this.organization = organization;
    }
}

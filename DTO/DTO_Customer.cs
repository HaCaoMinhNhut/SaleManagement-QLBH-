
namespace DTO
{
    public class DTO_Customer
    {
        private string id;
        private string name;
        private string address;
        private string phoneNumber;

        public DTO_Customer()
        {
        }

        public DTO_Customer(string id, string name, string address, string phoneNumber)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.phoneNumber = phoneNumber;
        }

        public DTO_Customer(string name, string address, string phoneNumber)
        {
            this.name = name;
            this.address = address;
            this.phoneNumber = phoneNumber;
        }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
    }
}

using System.Configuration;

namespace KpdApps.Orationi.Messaging.Core.Configurations.Rabbitmq
{
    public class RabbitmqConfigurationSection : ConfigurationSection
    {
        private const string SectionName = "rabbitmq";
        private const string HostNameKey = "hostName";
        private const string UserNameKey = "userName";
        private const string PasswordKey = "password";

        public static RabbitmqConfigurationSection GetConfiguration()
        {
            return (RabbitmqConfigurationSection)ConfigurationManager.GetSection(SectionName);
        }

        [ConfigurationProperty(HostNameKey)]
        public string HostName
        {
            get => (string)this[HostNameKey];
            set => this[HostNameKey] = (object)value;
        }

        [ConfigurationProperty(UserNameKey)]
        public string UserName
        {
            get => (string)this[UserNameKey];
            set => this[UserNameKey] = (object)value;
        }

        [ConfigurationProperty(PasswordKey)]
        public string Password
        {
            get => (string)this[PasswordKey];
            set => this[PasswordKey] = (object)value;
        }
    }
}

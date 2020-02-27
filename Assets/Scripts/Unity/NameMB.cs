namespace EntityComponentState.Unity
{
    public class NameMB : ComponentMB<Name>
    {
        public string value;

        private void Update()
        {
            // Saves on garbage collection
            if (value != name || component.name != name)
                value = component.name = name;
        }
    }
}
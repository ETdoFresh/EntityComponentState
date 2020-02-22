namespace EntityComponentState.Unity
{
    public class NameMB : ComponentMB<Name>
    {
        public string value;

        private void Update()
        {
            value = component.name = name;
        }
    }
}
namespace EntityComponentState.Unity
{
    public class NameMB : ComponentMB
    {
        public string value;
        public new Name name;

        private void OnEnable()
        {
            entity = GetComponent<EntityMB>().entity;
            if (name == null) name = new Name();
            entity.AddComponent(name);
        }

        private void OnDisable()
        {
            entity.RemoveComponent(name);
        }

        private void Update()
        {
            value = name.name = base.name;
        }
    }
}
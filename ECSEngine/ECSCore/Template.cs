using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECSCore
{
    public class Template
    {
        private Dictionary<string, EntityTempData> entityTemplates { get; set; } = new Dictionary<string, EntityTempData>();

        public Template()
        {
            GetEnumerableOfType<Entity>();
        }

        public T CreateTemplateObj<T>() where T : Entity, new()
        {
            T obj = new T();

            string name = typeof(T).Name;
            Component[] componentAttributes = (Component[]) Attribute.GetCustomAttributes(entityTemplates[name].EntityType, typeof(Component));

            for (int i = 0; i < componentAttributes.Length; i++)
            {
                obj.Components[entityTemplates[name].ComponentTypes[i]] = componentAttributes[i];
            }

            return obj;
        }

        // Reference from
        // https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class/6944605
        private void GetEnumerableOfType<T>() where T : Entity
        {
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                T obj = (T)Activator.CreateInstance(type, new object[0]);

                EntityTempData data = new EntityTempData();
                data.EntityType = obj.GetType();


                Component[] componentAttribute = (Component[]) Attribute.GetCustomAttributes(data.EntityType, typeof(Component));
                data.ComponentTypes = new string[componentAttribute.Length];

                for (int i = 0; i < componentAttribute.Length; i++)
                {
                    data.ComponentTypes[i] = componentAttribute[i].GetType().Name;
                }

                entityTemplates[type.Name] = data;
            }
        }
    }

    public class EntityTempData
    {
        public Type EntityType { get; set; }
        public string[] ComponentTypes { get; set; }
    }
}

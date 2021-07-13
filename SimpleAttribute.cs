using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SPA
{
    public static class MonoExt {
        public static void AutoLoad(this MonoBehaviour mono) {
            var fields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)  {
                AbsBaseAttribute attr = field.GetCustomAttribute(typeof(AbsBaseAttribute), true) as AbsBaseAttribute;
                if (attr != null)
                {
                    attr.Set(mono, field);
                }
                else {
                    Debug.LogError("attribute is null!!");
                }
            }
        }
    }
    //**************************************************************************************************//
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class AbsBaseAttribute : Attribute {
        public abstract void Set(MonoBehaviour mono, FieldInfo field);
    }
    public abstract class BaseAttribute : AbsBaseAttribute
    {
        public string name;
        public BaseAttribute(string name)
        {
            this.name = name;
        }
      
    }
    //**************************************************************************************************//
    public class NewGameObjectAttribute : BaseAttribute
    {
        public NewGameObjectAttribute(string name) : base(name) { }
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            var found = new GameObject(name);
            field.SetValue(mono, found);
        }
    }
    public class FindGameObjectAttribute : BaseAttribute
    {

        public FindGameObjectAttribute(string name) : base(name) { }
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            var found = GameObject.Find(name);
            field.SetValue(mono, found);
        }
    }
    public class FindChildAttribute : BaseAttribute
    {
        public FindChildAttribute(string name) : base(name) { }
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            var found = mono.gameObject.transform.Find(name);
            field.SetValue(mono, found);
        }
    }
    public class GetChildAttribute : BaseAttribute
    {
        public int index = 0;
        public GetChildAttribute(string name) : base(name) {
            this.index = int.Parse(name);
        }
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            var found = mono.gameObject.transform.GetChild(index);
            field.SetValue(mono, found);
        }
    }
    public class FindTagAttribute : BaseAttribute
    {
        public FindTagAttribute(string name) : base(name) {}
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            if (Array.Find(field.FieldType.GetInterfaces(), x => x == typeof(ICollection)) != null)
            {
                var found = GameObject.FindGameObjectsWithTag(name);
                field.SetValue(mono, found);
            }  else
            {
                var found = GameObject.FindGameObjectWithTag(name);
                field.SetValue(mono, found);
            }
        }
    }
    public class FindObjectAttribute : AbsBaseAttribute
    {
   
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            string typeName = field.FieldType.FullName.Replace("[]", string.Empty);
            Type type = field.FieldType.Assembly.GetType(typeName);
            if (Array.Find(field.FieldType.GetInterfaces(), x => x == typeof(ICollection)) != null)
            {
                var found = GameObject.FindObjectsOfType(type);
                field.SetValue(mono, found);
            }
            else
            {
                var found = GameObject.FindObjectOfType(type);
                field.SetValue(mono, found);
            }
          
        }
    }
    public class ComponentAttribute : AbsBaseAttribute
    {
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            var found = mono.GetComponent(field.FieldType);
            field.SetValue(mono, found);
        }
    }
    public class ComponentChildAttribute : AbsBaseAttribute
    {
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            string typeName = field.FieldType.FullName.Replace("[]", string.Empty);
           Type  type = field.FieldType.Assembly.GetType(typeName);

       
          //  Debug.Log(field.FieldType);

            if (field.FieldType.IsArray)
            {
              
                var found = mono.GetComponentsInChildren(type);
                int count = found.Length;
                IList listToBeSet = Array.CreateInstance(type, count);

                for (int index = 0; index < count; index++)
                    listToBeSet[index] = found[index];

               field.SetValue(mono, listToBeSet);
            } else
            {
                var found = mono.GetComponentInChildren(type);
                field.SetValue(mono, found);
            }
        }
    }
    public class ComponentParentAttribute : AbsBaseAttribute
    {
        public override void Set(MonoBehaviour mono, FieldInfo field)
        {
            var found = mono.GetComponentInParent(field.FieldType);
            field.SetValue(mono, found);
        }
    }


}

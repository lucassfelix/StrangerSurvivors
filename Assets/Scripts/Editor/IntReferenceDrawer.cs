using System.Linq;
using UnityEngine;
using UnityEditor;


namespace Editor
    {
    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position,label,property);
            bool useConstant = property.FindPropertyRelative("UsarConstante").boolValue;

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var rect = new Rect(position.position, Vector2.one * 20);
        
            if (EditorGUI.DropdownButton(rect, 
                new GUIContent(GetTexture())
                , FocusType.Keyboard, new GUIStyle()
                {fixedWidth = 50f, border = new RectOffset(1,1,1,1)}))
            {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Constante"),
                        useConstant,
                        () => SetProperty(property, true));
                    
                    menu.AddItem(new GUIContent("Variável"),
                        !useConstant,
                        () => SetProperty(property,false));
                    
                    menu.ShowAsContext();
            }

            position.position += Vector2.right * 15;

            int value = property.FindPropertyRelative("ValorConstante").intValue;

            if (useConstant)
            {
                string newValue = EditorGUI.TextField(position, value.ToString());
                int.TryParse(newValue, out value);
                property.FindPropertyRelative("ValorConstante").intValue = value;
            }
            else
            {
                EditorGUI.ObjectField(position,property.FindPropertyRelative("Variable"),GUIContent.none);
            }

            EditorGUI.EndProperty();
        }

        private void SetProperty(SerializedProperty property, bool value)
        {
            var propRelative = property.FindPropertyRelative("UsarConstante");
            propRelative.boolValue = value;
            property.serializedObject.ApplyModifiedProperties();
        }

        private Texture GetTexture()
        {
            var textures = Resources.FindObjectsOfTypeAll(typeof(Texture))
            .Where(t => t.name.ToLower().Contains("choose-remote-dark"))
            .Cast<Texture>().ToList();
            return textures[0];
        }
    }
}

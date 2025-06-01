# UIBinding

UIBinding is a library for setting up UI bindings with uGUI in Unity. Set up UI with only one line of code to Bind the object to the UI.

## Getting Started

### Installation

The Package was built in Unity 6, and requires UIToolkit inspectors for the Editor UI.

The tweens / animations depend on PrimeTween, get it for free: [Asset Store](https://api.unity.com/v1/oauth2/authorize?client_id=asset_store_v2&locale=en_GB&redirect_uri=https%3A%2F%2Fassetstore.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Fpackages%252Ftools%252Fanimation%252Fprimetween-high-performance-animations-and-sequences-252960&response_type=code&state=1a59d344-366a-4902-9702-4135d54fac77) [Github](https://github.com/KyryloKuzyk/PrimeTween?tab=readme-ov-file#install-via-unity-package-manager-upm)


In the package manager, select 'Install Package from Git URL' and use the url `https://github.com/PKelly429/UIBinding.git`

### Bindable Object

To create an object that can be bound to the UI, the `[Bindable]` attribute is added to the class. Only fields using Bindable types can be bound.

```c#
[Bindable]
public class Player : MonoBehaviour
{
    public BindableFloat health;
}
```

### UI

To begin binding to the UI, Add the UIBinding Component.

![image](https://github.com/user-attachments/assets/3dfdfa0b-7150-442a-bd0d-7432adf78ddd)

The Binding Type will allow you to select any object that has the `[Bindable]` attribute.

Using the dropdown, bindings can be added. e.g. to create a health bar using the player's health field, we can add the 'Image > Fill' binding and configuring it with the correct field and Image target.

![image](https://github.com/user-attachments/assets/a7a66a51-699a-48b4-94ca-e51c51387041)


### Binding

The UI needs to be told which object to bind to.

Getting a UI Reference:

```c#
public class UIReferences : MonoBehaviour
{
    public UIBinding playerBinding;
    
    public static UIReferences Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
}
```

![image](https://github.com/user-attachments/assets/ca92beaa-3fb7-45bb-98e9-78c694959c41)

Call `Bind()` with a reference to the object we want to bind.

```c#
[Bindable]
public class Player : MonoBehaviour
{
    public BindableFloat health;

    private void Start()
    {
        UIReferences.Instance.playerBinding.Bind(this);
    }
}
```

![image](https://github.com/user-attachments/assets/bba48521-ca21-40a6-9ba9-88b69d754402)

Update the health value, and the UI will update. Note: the values must be set using SetValue to activate the UI callbacks.

```c#
private void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        health.SetValue(Mathf.Max(health - 20, 0));
    }
}
```

![healthbarupdate](https://github.com/user-attachments/assets/d3be5da2-6459-4c8c-bb78-7ebd35b52cfa)

## Binding Types

### String Binders

Using the 'Complex Text: TMP' binder, we can setup text fields with a formatted string where the parameters are bound. Use `{0}` `{1}` etc, in the formatted string to add parameters.


![image](https://github.com/user-attachments/assets/6585da46-590a-4376-b3f3-1e931dc1c2c2)

![healthbarupdate2](https://github.com/user-attachments/assets/ccbef703-2504-4158-a19a-83a1c9d858c4)

### Follow Transform

The Follow Transform binder can make the UI follow an in world object, and can be used to easily create status tags, floating health bars or tooltips.

![image](https://github.com/user-attachments/assets/9faec676-5d69-4ab3-801e-dd3bf9b95e09)

![image](https://github.com/user-attachments/assets/6e79636d-d56f-4901-a8bc-8a8a2f538201)



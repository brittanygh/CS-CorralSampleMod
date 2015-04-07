using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using ICities;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using UnityEngine;

namespace CorralSampleMod
{
    public class CorralSampleMod : IUserMod
    {
       public string Description
       {
          get { return "This is a sample mod that shows usage of CorralMod"; }
       }

       public string Name
       {
          get { return "CorralSampleMod"; }
       }
    }

   public class CorralSampleLoading : ILoadingExtension
   {

      public void OnCreated(ILoading loading)
      {
      }

      public void OnLevelLoaded(LoadMode mode)
      {
         try // safety first!
         {
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
            {
               // find CorralMod object
               GameObject corralGo = GameObject.Find("CorralRegistrationGameObject");

               if (corralGo != null)
               {
                  // SendMessage takes two parameters,
                  // - the first parameter is the name of the methd to call "RegisterMod" in this case
                  // - the second parameter is an array of objects
                  // - array is one of exactly:
                  // - 4 elements (name, text, hovertext and callback)
                  // - 6 elements (previous, plus foreground sprite and texture)
                  // - 16 elements (previous, plus more foreground and background sprites and textures)
                  //
                  // - array elements:
                  // [0] - string name of mod
                  // [1] - string button text
                  // [2] - string hover text
                  // [3] - Action<string> delegate 
                  //
                  // normal sprites, foreground and background
                  //
                  // [4] - optional normalFg spritename (either builtin or custom)
                  // [5] - optional texture2d for normalFg spritename
                  // [6] - optional normalBg spritename (either builtin or custom)
                  // [7] - optional texture2d for normalBg spritename
                  //
                  // hovered sprites, foreground and background
                  //
                  // [8] - optional hoveredFg spritename (either builtin or custom)
                  // [9] - optional texture2d for hoveredFg spritename
                  // [10] - optional hoveredBg spritename (either builtin or custom)
                  // [11] - optional texture2d for hoveredBg spritename
                  //
                  // pressed sprites, foreground and background
                  //
                  // [12] - optional pressedFg spritename (either builtin or custom)
                  // [13] - optional texture2d for pressedFg spritename
                  // [14] - optional pressedBg spritename (either builtin or custom)
                  // [15] - optional texture2d for pressedBg spritename      

                  // simple example 1
                  // - no sprites, member method callback
                  // params are mod name, button text, hover text, callback
                  ModParamHelper mph = new ModParamHelper("CorralSampleMod", "Test A", "This is Corral Sample button 1", (Action<string>)this.Callback1);
                  corralGo.SendMessage("RegisterMod", mph.GetParams());

                  // simple example 1.5
                  // - no sprites, member method callback
                  // params are mod name, button text, hover text, callback
                  mph = new ModParamHelper("CorralSampleMod", "+", "This is Corral Sample button 1.5", (Action<string>)this.Callback1);
                  corralGo.SendMessage("RegisterMod", mph.GetParams());

                  // example 2
                  // - simple builtin sprite name (see: http://docs.skylinesmodding.com/en/master/resources/UI-Sprites-1.html for names)
                  // - anon delegate callback   
                  // params are like example 1, plus foreground spritename
                  mph = new ModParamHelper("CorralSampleMod", "Button2", "This is Corral Sample button 2", (Action<string>)delegate(string s2) { MessageBox(s2); }, "IconAssetVehicle", null);
                  corralGo.SendMessage("RegisterMod", mph.GetParams());

                  // example 3
                  // - multiple builtin sprite names
                  // - anon delegate callback               
                  // - like example 2, plus all spritenames
                  mph = new ModParamHelper("CorralSampleMod", "Button3", "This is Corral Sample button 3", (Action<string>)delegate(string s3) { MessageBox(s3); }, 
                     "InfoIconPublicTransport", null,  
                     "InfoIconBaseNormal",  null, 
                     "InfoIconPublicTransportHovered",  null, 
                     "InfoIconBaseHovered",  null, 
                     "InfoIconPublicTransportPressed",  null, 
                     "InfoIconBasePressed", null);
                  corralGo.SendMessage("RegisterMod", mph.GetParams());

                  // example 4
                  // - custom texture for foreground
                  // - anon delegate callback               
                  // load a texture from resource embedded in this mod dll.  you could load from a file as well
                  System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.Canada-Flag-icon.png"); // namespace and filename of a png I embedded

                  byte[] bytes = new byte[s.Length];
                  int numBytesToRead = (int)s.Length;
                  int n = s.Read(bytes, 0, numBytesToRead);

                  s.Close();

                  Texture2D mytex = new Texture2D(2, 2);
                  mytex.LoadImage(bytes); // this will auto-resize the texture dimensions

                  mph = new ModParamHelper("CorralSampleMod", "Button4", "This is Corral Sample button 4", (Action<string>)delegate(string s4) { MessageBox(s4); }, "my_sample4_sprite", mytex);
                  corralGo.SendMessage("RegisterMod", mph.GetParams());

                  // Toggle buttons take 28 parameters in the object array and the callback has an int parameter with either 0 or 1 as its value
                  // 0 means state 0 (the initial 'unactivated' state)
                  // 1 means state 1 (the next 'activated' state)
                  // 
                  // array must be of the form:
                  // [0] - string name of mod
                  // [1] - string button text
                  // [2] - string hover text
                  // [3] - Action<string, int> delegate.  string is modname+buttontext identifier, bool is current state flag (true=pressed, false=normal/unpressed)
                  //
                  // State 0 Sprites
                  //--------------------
                  // normal sprites, foreground and background
                  //
                  //     [4] - mandatory normalFg spritename (either builtin or custom)
                  //     [5] - optional texture2d for normalFg spritename
                  //     [6] - mandatory normalBg spritename (either builtin or custom)
                  //     [7] - optional texture2d for normalBg spritename
                  //
                  //     hovered sprites, foreground and background
                  //
                  //     [8] - mandatory hoveredFg spritename (either builtin or custom)
                  //     [9] - optional texture2d for hoveredFg spritename
                  //     [10] - mandatory hoveredBg spritename (either builtin or custom)
                  //     [11] - optional texture2d for hoveredBg spritename
                  //
                  //     pressed sprites, foreground and background
                  //
                  //     [12] - mandatory pressedFg spritename (either builtin or custom)
                  //     [13] - optional texture2d for pressedFg spritename
                  //     [14] - mandatory pressedBg spritename (either builtin or custom)
                  //     [15] - optional texture2d for pressedBg spritename    
                  //
                  // State 1 Sprites
                  //--------------------
                  // normal sprites, foreground and background
                  //
                  //     [16] - mandatory normalFg spritename (either builtin or custom)
                  //     [17] - optional texture2d for normalFg spritename
                  //     [18] - mandatory normalBg spritename (either builtin or custom)
                  //     [19] - optional texture2d for normalBg spritename
                  //
                  //     hovered sprites, foreground and background
                  //
                  //     [20] - mandatory hoveredFg spritename (either builtin or custom)
                  //     [21] - optional texture2d for hoveredFg spritename
                  //     [22] - mandatory hoveredBg spritename (either builtin or custom)
                  //     [23] - optional texture2d for hoveredBg spritename
                  //
                  //     pressed sprites, foreground and background
                  //
                  //     [24] - mandatory pressedFg spritename (either builtin or custom)
                  //     [25] - optional texture2d for pressedFg spritename
                  //     [26] - mandatory pressedBg spritename (either builtin or custom)
                  //     [27] - optional texture2d for pressedBg spritename    

                  // example 5
                  // - a toggle button, which retains a state of being pressed or not pressed
                  // - note the different method name being passed to SendMessage()
                  // 
                  ToggleParamHelper tph = new ToggleParamHelper("CorralSampleMod", "Button5", "this is a corral sample toggle button", (Action<string, int>)delegate(string s5, int i5) { ToggleMessageBox(s5, i5); }, 
                     // state 0
                     "InfoIconElectricity", null,
                     "InfoIconBaseNormal", null,
                     "InfoIconElectricity", null, 
                     "InfoIconBaseHovered", null,
                     "InfoIconElectricity", null, 
                     "InfoIconBasePressed", null,
                     // state 1
                     "InfoIconElectricity", null,
                     "TutorialGlow", null,
                     "InfoIconElectricity", null, 
                     "InfoIconBaseHovered", null,
                     "InfoIconElectricity", null, 
                     "InfoIconBasePressed", null
                     );
                  corralGo.SendMessage("RegisterModToggleButton", tph.GetParams());

                  // example 6
                  // - a toggle button, which retains a state of being pressed or not pressed
                  // - using custom textures
                  // 
                  s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.state0_normal_fg.png"); // namespace and filename of a png I embedded
                  bytes = new byte[s.Length];
                  numBytesToRead = (int)s.Length;
                  n = s.Read(bytes, 0, numBytesToRead);
                  s.Close();
                  Texture2D s0normal = new Texture2D(2, 2);
                  s0normal.LoadImage(bytes); // this will auto-resize the texture dimensions

                  s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.state0_hovered_fg.png"); // namespace and filename of a png I embedded
                  bytes = new byte[s.Length];
                  numBytesToRead = (int)s.Length;
                  n = s.Read(bytes, 0, numBytesToRead);
                  s.Close();
                  Texture2D s0hovered = new Texture2D(2, 2);
                  s0hovered.LoadImage(bytes); // this will auto-resize the texture dimensions

                  s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.state0_pressed_fg.png"); // namespace and filename of a png I embedded
                  bytes = new byte[s.Length];
                  numBytesToRead = (int)s.Length;
                  n = s.Read(bytes, 0, numBytesToRead);
                  s.Close();
                  Texture2D s0pressed = new Texture2D(2, 2);
                  s0pressed.LoadImage(bytes); // this will auto-resize the texture dimensions

                  s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.state1_normal_fg.png"); // namespace and filename of a png I embedded
                  bytes = new byte[s.Length];
                  numBytesToRead = (int)s.Length;
                  n = s.Read(bytes, 0, numBytesToRead);
                  s.Close();
                  Texture2D s1normal = new Texture2D(2, 2);
                  s1normal.LoadImage(bytes); // this will auto-resize the texture dimensions

                  s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.state1_hovered_fg.png"); // namespace and filename of a png I embedded
                  bytes = new byte[s.Length];
                  numBytesToRead = (int)s.Length;
                  n = s.Read(bytes, 0, numBytesToRead);
                  s.Close();
                  Texture2D s1hovered = new Texture2D(2, 2);
                  s1hovered.LoadImage(bytes); // this will auto-resize the texture dimensions

                  s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.state1_pressed_fg.png"); // namespace and filename of a png I embedded
                  bytes = new byte[s.Length];
                  numBytesToRead = (int)s.Length;
                  n = s.Read(bytes, 0, numBytesToRead);
                  s.Close();
                  Texture2D s1pressed = new Texture2D(2, 2);
                  s1pressed.LoadImage(bytes); // this will auto-resize the texture dimensions

                  tph = new ToggleParamHelper("CorralSampleMod", "Button6", "this is a custom textue toggle button", (Action<string, int>)delegate(string s6, int i6) { ToggleMessageBox(s6, i6); },
                     // state 0
                     "s0_normalfg", s0normal,
                     "", null,
                     "s0_hovered", s0hovered,
                     "", null,
                     "s0_pressed", s0pressed,
                     "", null,
                     // state 1
                     "s1_normalfg", s1normal,
                     "", null,
                     "s1_hovered", s1hovered,
                     "", null,
                     "s1_pressed", s1pressed,
                     "", null
                     );
                  corralGo.SendMessage("RegisterModToggleButton", tph.GetParams());

               }
               else
               {
                  DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "Cound not find corral gameobject...");
               }
            }
         }
         catch (Exception ex)
         {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, "CorralSampleMod exception! " + ex.Message);
         }
      }

      public void OnLevelUnloading()
      {
      }

      public void OnReleased()
      {
      }

      public void Callback1(string buttonName)
      {
         MessageBox("I'm a callback from " + buttonName);
      }

      public void MessageBox(string msg)
      {
         UIView uiv = UIView.GetAView();

         if (uiv == null)
         {
            return;
         }

         UILabel label =(UILabel)uiv.AddUIComponent(typeof(UILabel));
         label.autoSize = true;
         label.text = msg + "\n\n<click on me to close>";
         label.wordWrap = true;
         label.backgroundSprite = "GenericPanel";
         label.padding = new RectOffset(15, 15, 15, 15);
         label.CenterToParent();

         label.eventClick += (component, param) => 
         { 
            label.Hide(); 
            if (label.parent != null)
            {
               label.parent.RemoveUIComponent(label);
               UnityEngine.Object.Destroy(label);
            }
         };
      }

      public void ToggleMessageBox(string msg, int stateIndex)
      {
         UIView uiv = UIView.GetAView();

         if (uiv == null)
         {
            return;
         }

         UILabel label = (UILabel)uiv.AddUIComponent(typeof(UILabel));
         label.autoSize = true;
         label.text = string.Format("Toggle Button\n\n{0}\n\nstate is {1}\n\n<click on me to close>", msg, stateIndex);
         label.wordWrap = true;
         label.backgroundSprite = "GenericPanel";
         label.padding = new RectOffset(15, 15, 15, 15);
         label.CenterToParent();

         label.eventClick += (component, param) =>
         {
            label.Hide();
            if (label.parent != null)
            {
               label.parent.RemoveUIComponent(label);
               UnityEngine.Object.Destroy(label);
            }
         };
      }
   }

   // helper class for param creation
   public class ModParamHelper
   {
      protected string ModName;
      protected string ButtonText;
      protected string HoverText;
      protected Action<string> ClickCallback;

      protected string NormalFgSpritename;
      protected Texture2D NormalFgTexture;
      protected string NormalBgSpritename;
      protected Texture2D NormalBgTexture;

      protected string HoveredFgSpritename;
      protected Texture2D HoveredFgTexture;
      protected string HoveredBgSpritename;
      protected Texture2D HoveredBgTexture;

      protected string PressedFgSpritename;
      protected Texture2D PressedFgTexture;
      protected string PressedBgSpritename;
      protected Texture2D PressedBgTexture;

      protected int size = 0;

      // minimal parameters
      public ModParamHelper(string modName, string buttonText, string hoverText, Action<string> callback)
      {
         ModName = modName;
         ButtonText = buttonText;
         HoverText = hoverText;
         ClickCallback = callback;
         size = 4;
      }

      // next set of parameters
      public ModParamHelper(string modName, string buttonText, string hoverText, Action<string> callback, string normalFgSpritename, Texture2D normalFgTexture)
      {
         ModName = modName;
         ButtonText = buttonText;
         HoverText = hoverText;
         ClickCallback = callback;
         NormalFgSpritename = normalFgSpritename;
         NormalFgTexture = normalFgTexture;
         size = 6;
      }

      // maximal set of parameters
      public ModParamHelper(string modName, string buttonText, string hoverText, Action<string> callback, 
         string normalFgSpritename, Texture2D normalFgTexture,
         string normalBgSpritename, Texture2D normalBgTexture,
         string hoveredFgSpritename, Texture2D hoveredFgTexture,
         string hoveredBgSpritename, Texture2D hoveredBgTexture,
         string pressedFgSpritename, Texture2D pressedFgTexture,
         string pressedBgSpritename, Texture2D pressedBgTexture)
      {
         ModName = modName;
         ButtonText = buttonText;
         HoverText = hoverText;
         ClickCallback = callback;
         NormalFgSpritename = normalFgSpritename;
         NormalFgTexture = normalFgTexture;
         NormalBgSpritename = normalBgSpritename;
         NormalBgTexture = normalBgTexture;
         HoveredFgSpritename = hoveredFgSpritename;
         HoveredFgTexture = hoveredFgTexture;
         HoveredBgSpritename = hoveredBgSpritename;
         HoveredBgTexture = hoveredBgTexture;
         PressedFgSpritename = pressedFgSpritename;
         PressedFgTexture = pressedFgTexture;
         PressedBgSpritename = pressedBgSpritename;
         PressedBgTexture = pressedBgTexture;
         size = 16;
      }

      // generate param array
      public virtual object[] GetParams()
      {
         if (size == 0)
            return null;

         object[] array = new object[size];

         if (size >= 4) // always
         {
            array[0] = ModName;
            array[1] = ButtonText;
            array[2] = HoverText;
            array[3] = ClickCallback;
         }

         if (size >= 6)
         {
            array[4] = NormalFgSpritename;
            array[5] = NormalFgTexture;
         }

         if (size >= 16)
         {
            array[6] = NormalBgSpritename;
            array[7] = NormalBgTexture;
            array[8] = HoveredFgSpritename;
            array[9] = HoveredFgTexture;
            array[10] = HoveredBgSpritename;
            array[11] = HoveredBgTexture;
            array[12] = PressedFgSpritename;
            array[13] = PressedFgTexture;
            array[14] = PressedBgSpritename;
            array[15] = PressedBgTexture;
         }

         return array;
      }
   }

   public class ToggleParamHelper : ModParamHelper
   {
      protected Action<string, int> ToggleCallback;

      // state 1 sprites
      protected string NormalFgSpritename_state1;
      protected Texture2D NormalFgTexture_state1;
      protected string NormalBgSpritename_state1;
      protected Texture2D NormalBgTexture_state1;

      protected string HoveredFgSpritename_state1;
      protected Texture2D HoveredFgTexture_state1;
      protected string HoveredBgSpritename_state1;
      protected Texture2D HoveredBgTexture_state1;

      protected string PressedFgSpritename_state1;
      protected Texture2D PressedFgTexture_state1;
      protected string PressedBgSpritename_state1;
      protected Texture2D PressedBgTexture_state1;

      // minimal set of parameters
      public ToggleParamHelper(string modName, string buttonText, string hoverText, Action<string, int> callback, 
         string normalFgSpritename, Texture2D normalFgTexture,
         string normalBgSpritename, Texture2D normalBgTexture,
         string hoveredFgSpritename, Texture2D hoveredFgTexture,
         string hoveredBgSpritename, Texture2D hoveredBgTexture,
         string pressedFgSpritename, Texture2D pressedFgTexture,
         string pressedBgSpritename, Texture2D pressedBgTexture,
         string normalFgSpritename_state1, Texture2D normalFgTexture_state1,
         string normalBgSpritename_state1, Texture2D normalBgTexture_state1,
         string hoveredFgSpritename_state1, Texture2D hoveredFgTexture_state1,
         string hoveredBgSpritename_state1, Texture2D hoveredBgTexture_state1,
         string pressedFgSpritename_state1, Texture2D pressedFgTexture_state1,
         string pressedBgSpritename_state1, Texture2D pressedBgTexture_state1
         )
         : base(modName, buttonText, hoverText, null, 
               normalFgSpritename, normalFgTexture,
               normalBgSpritename, normalBgTexture,
               hoveredFgSpritename, hoveredFgTexture,
               hoveredBgSpritename, hoveredBgTexture,
               pressedFgSpritename, pressedFgTexture,
               pressedBgSpritename, pressedBgTexture)
      {
         ToggleCallback = callback;

         NormalFgSpritename_state1 = normalFgSpritename_state1;
         NormalFgTexture_state1 = normalFgTexture_state1;
         NormalBgSpritename_state1 = normalBgSpritename_state1;
         NormalBgTexture_state1 = normalBgTexture_state1;
         HoveredFgSpritename_state1 = hoveredFgSpritename_state1;
         HoveredFgTexture_state1 = hoveredFgTexture_state1;
         HoveredBgSpritename_state1 = hoveredBgSpritename_state1;
         HoveredBgTexture_state1 = hoveredBgTexture_state1;
         PressedFgSpritename_state1 = pressedFgSpritename_state1;
         PressedFgTexture_state1 = pressedFgTexture_state1;
         PressedBgSpritename_state1 = pressedBgSpritename_state1;
         PressedBgTexture_state1 = pressedBgTexture_state1;

         size = 28;
      }

      public override object[] GetParams()
      {
         object[] array = base.GetParams();

         if (array != null)
         {
            array[3] = ToggleCallback;
         }

         if (size >= 28) // always
         {
            array[16] = NormalFgSpritename_state1;
            array[17] = NormalFgTexture_state1;
            array[18] = NormalBgSpritename_state1;
            array[19] = NormalBgTexture_state1;
            array[20] = HoveredFgSpritename_state1;
            array[21] = HoveredFgTexture_state1;
            array[22] = HoveredBgSpritename_state1;
            array[23] = HoveredBgTexture_state1;
            array[24] = PressedFgSpritename_state1;
            array[25] = PressedFgTexture_state1;
            array[26] = PressedBgSpritename_state1;
            array[27] = PressedBgTexture_state1;
         }

         return array;
      }
   }
}

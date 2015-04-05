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
                  object[] paramArray = new object[4];
                  paramArray[0] = "CorralSampleMod"; // our mod name
                  paramArray[1] = "Button1 test text"; // button name
                  paramArray[2] = "This is Corral Sample button 1";
                  paramArray[3] = (Action<string>)this.Callback1;
                  corralGo.SendMessage("RegisterMod", paramArray);

                  // example 2
                  // - simple builtin sprite name (see: http://docs.skylinesmodding.com/en/master/resources/UI-Sprites-1.html for names)
                  // - anon delegate callback               
                  object[] paramArray2 = new object[6];
                  paramArray2[0] = "CorralSampleMod"; // our mod name
                  paramArray2[1] = "Button2"; // button name
                  paramArray2[2] = "This is Corral Sample button 2";
                  paramArray2[3] = (Action<string>)delegate(string s2) { MessageBox(s2); };
                  paramArray2[4] = "IconAssetVehicle"; // just some random built-in sprite name
                  paramArray2[5] = null; // not using a custom texture
                  corralGo.SendMessage("RegisterMod", paramArray2);

                  // example 3
                  // - multiple builtin sprite names
                  // - anon delegate callback               
                  object[] paramArray3 = new object[16];
                  paramArray3[0] = "CorralSampleMod"; // our mod name
                  paramArray3[1] = "Button3"; // button name
                  paramArray3[2] = "This is Corral Sample button 3";
                  paramArray3[3] = (Action<string>)delegate(string s3) { MessageBox(s3); };

                  paramArray3[4] = "InfoIconPublicTransport"; // normal foreground sprite
                  paramArray3[5] = null; // not using a custom texture
                  paramArray3[6] = "InfoIconBaseNormal"; // normal background sprite
                  paramArray3[7] = null; // not using a custom texture

                  paramArray3[8] = "InfoIconPublicTransportHovered"; // hovered foreground sprite
                  paramArray3[9] = null; // not using a custom texture
                  paramArray3[10] = "InfoIconBaseHovered"; // hovered background sprite
                  paramArray3[11] = null; // not using a custom texture

                  paramArray3[12] = "InfoIconPublicTransportPressed"; // pressed foreground sprite
                  paramArray3[13] = null; // not using a custom texture
                  paramArray3[14] = "InfoIconBasePressed"; // pressed background sprite
                  paramArray3[15] = null; // not using a custom texture

                  corralGo.SendMessage("RegisterMod", paramArray3);

                  // example 4
                  // - custom texture for foreground
                  // - anon delegate callback               
                  object[] paramArray4 = new object[6];
                  paramArray4[0] = "CorralSampleMod"; // our mod name
                  paramArray4[1] = "Button4"; // button name
                  paramArray4[2] = "This is Corral Sample button 4";
                  paramArray4[3] = (Action<string>)delegate(string s4) { MessageBox(s4); };

                  // load a texture from resource embedded in this mod dll.  you could load from a file as well
                  System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CorralSampleMod.Canada-Flag-icon.png"); // namespace and filename of a png I embedded

                  if (s == null) DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "stream is null");

                  byte[] bytes = new byte[s.Length];
                  int numBytesToRead = (int)s.Length;
                  int n = s.Read(bytes, 0, numBytesToRead);

                  DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "stream length: " + s.Length.ToString() + " read bytes: " + n.ToString());

                  s.Close();

                  Texture2D mytex = new Texture2D(2, 2);
                  mytex.LoadImage(bytes); // this will auto-resize the texture dimensions

                  paramArray4[4] = "my_sample4_sprite"; // can be any unique string
                  paramArray4[5] = mytex;
                  corralGo.SendMessage("RegisterMod", paramArray4);

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

         //theBannerPanel = (BannerPanel)uiv.AddUIComponent(typeof(BannerPanel));

      }

   }
}

﻿using GadgetCore.API;
using GadgetCore.API.ConfigMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UModFramework.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GadgetCore
{
    internal static class SceneInjector
    {
        public static GameObject ModMenuBeam { get; internal set; }
        public static GameObject ModMenuButtonHolder { get; internal set; }
        public static GameObject ModMenu { get; internal set; }
        public static Canvas ModMenuCanvas { get; internal set; }
        public static RectTransform ModMenuPanel { get; internal set; }
        public static RectTransform ModConfigMenus { get; internal set; }
        public static GameObject ModMenuBackButtonBeam { get; internal set; }
        public static GameObject ModMenuBackButtonHolder { get; internal set; }
        public static GameObject ModConfigMenuText { get; internal set; }
        public static ModDescPanelController ModMenuDescPanel { get; internal set; }

        public static Sprite BoxSprite { get; internal set; }
        public static Sprite BoxMask { get; internal set; }
        public static Sprite BarSprite { get; internal set; }

        internal static void InjectMainMenu()
        {
            GadgetCore.Log("Injecting Mod Menu into Main Menu");

            GameObject mainMenu = InstanceTracker.Menuu.menuMain;
            Array.ForEach(mainMenu.GetComponentsInChildren<Animation>(), x => x.enabled = true);
            ModMenuBeam = UnityEngine.Object.Instantiate(mainMenu.transform.Find("beamm").gameObject, mainMenu.transform);
            ModMenuBeam.name = "beamm";
            ModMenuBeam.transform.localScale = new Vector3(30, 0, 1);
            ModMenuBeam.transform.position = new Vector3(0, -13.5f, 1);
            ModMenuButtonHolder = UnityEngine.Object.Instantiate(mainMenu.transform.Find("BUTTONHOLDER").gameObject, mainMenu.transform);
            ModMenuButtonHolder.name = "BUTTONHOLDER";
            ModMenuButtonHolder.transform.position = new Vector3(0, -13.5f, 0);
            ModMenuButtonHolder.GetComponent<Animation>().RemoveClip("enterr1");
            ModMenuButtonHolder.GetComponent<Animation>().AddClip(BuildModMenuButtonAnimClip(false), "enterr1");
            ModMenuButtonHolder.GetComponent<Animation>().clip = ModMenuButtonHolder.GetComponent<Animation>().GetClip("enterr1");
            GameObject bModMenu = ModMenuButtonHolder.transform.GetChild(0).gameObject;
            bModMenu.name = "bModMenu";
            Array.ForEach(bModMenu.GetComponentsInChildren<TextMesh>(), x => x.text = GadgetCore.IsUnpacked ? "MOD MENU" : File.Exists(Path.Combine(UMFData.ModsPath, "GadgetCore.dll")) ? "UPDATE GADGET CORE" : "UNPACK GADGET CORE");
            ModMenuBeam.GetComponent<Animation>().Play();
            ModMenuButtonHolder.GetComponent<Animation>().Play();
            if (GadgetCore.IsUnpacked) BuildModMenu();
        }

        internal static void InjectIngame()
        {
            GadgetCoreAPI.menus = new List<GameObject>();
            for (int i = 0;i < GadgetCoreAPI.menuPaths.Count;i++)
            {
                GameObject menu = Resources.Load(GadgetCoreAPI.menuPaths[i]) as GameObject;
                menu.transform.SetParent(InstanceTracker.MainCamera.transform);
                menu.SetActive(false);
                GadgetCoreAPI.menus.Add(menu);
            }
        }

        private static AnimationClip BuildModMenuButtonAnimClip(bool reverse)
        {
            int reverseMult = reverse ? -1 : 1;
            AnimationClip clip = new AnimationClip
            {
                legacy = true
            };
            clip.SetCurve("", typeof(Transform), "m_LocalPosition.x", new AnimationCurve(new Keyframe(0f, reverseMult * 26.09399f, 288.2641f, 288.2641f), new Keyframe(0.1f, reverseMult * -2.732423f, 123.6389f, 123.6389f), new Keyframe(0.1666667f, 0f, -40.98634f, -40.98634f)));
            clip.SetCurve("", typeof(Transform), "m_LocalPosition.y", AnimationCurve.Linear(0, -13.5f, 0.1666667f, -13.5f));
            return clip;
        }

        private static void BuildModMenu()
        {
            ModMenu = new GameObject("MODMENU");
            ModMenu.SetActive(false);
            ModMenuBackButtonBeam = UnityEngine.Object.Instantiate(InstanceTracker.Menuu.menuOptions.transform.Find("beamm").gameObject, ModMenu.transform);
            ModMenuBackButtonBeam.name = "beamm";
            ModMenuBackButtonBeam.transform.localScale = new Vector3(30, 0, 1);
            ModMenuBackButtonBeam.transform.position = new Vector3(0, -13.5f, 1);
            ModMenuBackButtonHolder = UnityEngine.Object.Instantiate(InstanceTracker.Menuu.menuOptions.transform.Find("BUTTONHOLDER").gameObject, ModMenu.transform);
            ModMenuBackButtonHolder.name = "BUTTONHOLDER";
            ModMenuBackButtonHolder.transform.position = new Vector3(0, -13.5f, 0);
            ModMenuBackButtonHolder.GetComponent<Animation>().RemoveClip("bbbac2");
            ModMenuBackButtonHolder.GetComponent<Animation>().AddClip(BuildModMenuButtonAnimClip(true), "bbbac2");
            ModMenuBackButtonHolder.GetComponent<Animation>().clip = ModMenuBackButtonHolder.GetComponent<Animation>().GetClip("bbbac2");
            ModMenuCanvas = new GameObject("Mod Menu Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup)).GetComponent<Canvas>();
            ModMenuCanvas.GetComponent<CanvasGroup>().alpha = 0;
            ModMenuCanvas.GetComponent<CanvasGroup>().interactable = false;
            ModMenuCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ModMenuCanvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            ModMenuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            ModMenuCanvas.pixelPerfect = true;
            CanvasScaler scaler = ModMenuCanvas.GetComponent<CanvasScaler>();
            scaler.scaleFactor = 2;
            scaler.referencePixelsPerUnit = 100;
            ModConfigMenuText = UnityEngine.Object.Instantiate(InstanceTracker.Menuu.menuOptions.transform.Find("txt0").gameObject, ModMenu.transform);
            ModConfigMenuText.name = "txt0";
            ModConfigMenuText.transform.localPosition = new Vector3(0, 14, -1);
            Array.ForEach(ModConfigMenuText.GetComponentsInChildren<TextMesh>(), x => { x.text = "MOD CONFIG MENU"; x.anchor = TextAnchor.UpperCenter; });
            GameObject restartRequiredText = UnityEngine.Object.Instantiate(ModConfigMenuText, ModMenu.transform);
            restartRequiredText.SetActive(false);
            restartRequiredText.name = "Restart Required Text";
            restartRequiredText.transform.localPosition = new Vector3(0, -10.5f, -1);
            restartRequiredText.transform.localScale *= 0.75f;
            Array.ForEach(restartRequiredText.GetComponentsInChildren<TextMesh>(), x => { x.text = "Restart Required!"; x.anchor = TextAnchor.UpperCenter; });
            ModMenuPanel = new GameObject("Panel", typeof(RectTransform)).GetComponent<RectTransform>();
            ModMenuPanel.SetParent(ModMenuCanvas.transform);
            ModMenuPanel.anchorMin = new Vector2(0.15f, 0.15f);
            ModMenuPanel.anchorMax = new Vector2(0.85f, 0.85f);
            ModMenuPanel.offsetMin = Vector2.zero;
            ModMenuPanel.offsetMax = Vector2.zero;
            ModConfigMenus = new GameObject("Mod Config Menus", typeof(RectTransform)).GetComponent<RectTransform>();
            ModConfigMenus.SetParent(ModMenuCanvas.transform);
            ModConfigMenus.anchorMin = new Vector2(0.15f, 0.15f);
            ModConfigMenus.anchorMax = new Vector2(0.85f, 0.85f);
            ModConfigMenus.offsetMin = Vector2.zero;
            ModConfigMenus.offsetMax = Vector2.zero;
            Image background = new GameObject("Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)).GetComponent<Image>();
            background.transform.SetParent(ModMenuPanel);
            background.rectTransform.anchorMin = new Vector2(0f, 0f);
            background.rectTransform.anchorMax = new Vector2(01f, 1f);
            background.rectTransform.offsetMin = Vector2.zero;
            background.rectTransform.offsetMax = Vector2.zero;
            background.type = Image.Type.Sliced;
            background.fillCenter = true;
            Texture2D boxTex = GadgetCoreAPI.LoadTexture2D("boxsprite.png");
            boxTex.filterMode = FilterMode.Point;
            Texture2D boxMaskTex = GadgetCoreAPI.LoadTexture2D("boxmask.png");
            boxMaskTex.filterMode = FilterMode.Point;
            Texture2D barTex = GadgetCoreAPI.LoadTexture2D("barsprite.png");
            barTex.filterMode = FilterMode.Point;
            BoxSprite = Sprite.Create(boxTex, new Rect(0, 0, boxTex.width, boxTex.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, new Vector4(15, 15, 15, 15));
            BoxMask = Sprite.Create(boxMaskTex, new Rect(0, 0, boxMaskTex.width, boxMaskTex.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, new Vector4(15, 15, 15, 15));
            BarSprite = Sprite.Create(barTex, new Rect(0, 0, barTex.width, barTex.height), new Vector2(0.5f, 0.5f), 100, 1, SpriteMeshType.Tight, new Vector4(1, 1, 1, 1));
            background.sprite = BoxSprite;

            EventSystem eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule)).GetComponent<EventSystem>();
            StandaloneInputModule inputModule = eventSystem.GetComponent<StandaloneInputModule>();
            inputModule.horizontalAxis = "Horizontal1";
            inputModule.verticalAxis = "Vertical1";
            inputModule.submitButton = "Jump";
            inputModule.cancelButton = "Cancel";

            GadgetModConfigs.BuildConfigMenus(ModConfigMenus);

            ModMenuDescPanel = new GameObject("Mod Desc Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(ModDescPanelController), typeof(ScrollRect)).GetComponent<ModDescPanelController>();
            ModMenuDescPanel.GetComponent<RectTransform>().SetParent(ModMenuPanel);
            ModMenuDescPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.4f, 0.25f);
            ModMenuDescPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            ModMenuDescPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0, 5);
            ModMenuDescPanel.GetComponent<RectTransform>().offsetMax = new Vector2(-10, -10);
            ModMenuDescPanel.GetComponent<Image>().sprite = BoxSprite;
            ModMenuDescPanel.GetComponent<Image>().type = Image.Type.Sliced;
            ModMenuDescPanel.GetComponent<Image>().fillCenter = true;
            Mask modMenuDescPanelMask = new GameObject("Mask", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Mask)).GetComponent<Mask>();
            modMenuDescPanelMask.GetComponent<RectTransform>().SetParent(ModMenuDescPanel.transform);
            modMenuDescPanelMask.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            modMenuDescPanelMask.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            modMenuDescPanelMask.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            modMenuDescPanelMask.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            modMenuDescPanelMask.GetComponent<Image>().sprite = BoxMask;
            modMenuDescPanelMask.GetComponent<Image>().type = Image.Type.Sliced;
            modMenuDescPanelMask.GetComponent<Image>().fillCenter = true;
            modMenuDescPanelMask.showMaskGraphic = false;
            RectTransform modMenuDescViewport = new GameObject("Viewport", typeof(RectTransform)).GetComponent<RectTransform>();
            modMenuDescViewport.SetParent(modMenuDescPanelMask.transform);
            modMenuDescViewport.anchorMin = new Vector2(0f, 0f);
            modMenuDescViewport.anchorMax = new Vector2(1f, 1f);
            modMenuDescViewport.offsetMin = new Vector2(10, 10);
            modMenuDescViewport.offsetMax = new Vector2(-10, -10);
            Text modMenuDescText = new GameObject("Description", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(ContentSizeFitter)).GetComponent<Text>();
            modMenuDescText.rectTransform.SetParent(modMenuDescViewport);
            modMenuDescText.rectTransform.anchorMin = new Vector2(0f, 0f);
            modMenuDescText.rectTransform.anchorMax = new Vector2(1f, 1f);
            modMenuDescText.rectTransform.offsetMin = Vector2.zero;
            modMenuDescText.rectTransform.offsetMax = Vector2.zero;
            modMenuDescText.rectTransform.pivot = new Vector2(0.5f, 1f);
            modMenuDescText.font = ModConfigMenuText.GetComponent<TextMesh>().font;
            modMenuDescText.fontSize = 12;
            modMenuDescText.horizontalOverflow = HorizontalWrapMode.Wrap;
            modMenuDescText.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            ModMenuDescPanel.GetComponent<ScrollRect>().content = modMenuDescText.rectTransform;
            ModMenuDescPanel.GetComponent<ScrollRect>().horizontal = false;
            ModMenuDescPanel.GetComponent<ScrollRect>().scrollSensitivity = 5;
            ModMenuDescPanel.GetComponent<ScrollRect>().viewport = modMenuDescViewport;
            ModMenuDescPanel.descText = modMenuDescText;
            ModMenuDescPanel.restartRequiredText = restartRequiredText;
            Image modMenuButtonPanel = new GameObject("Button Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)).GetComponent<Image>();
            modMenuButtonPanel.GetComponent<RectTransform>().SetParent(ModMenuPanel);
            modMenuButtonPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.4f, 0f);
            modMenuButtonPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.25f);
            modMenuButtonPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0, 10);
            modMenuButtonPanel.GetComponent<RectTransform>().offsetMax = new Vector2(-10, -5);
            modMenuButtonPanel.GetComponent<Image>().sprite = BoxSprite;
            modMenuButtonPanel.GetComponent<Image>().type = Image.Type.Sliced;
            modMenuButtonPanel.GetComponent<Image>().fillCenter = true;
            ModMenuDescPanel.configButton = new GameObject("Config Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).GetComponent<Button>();
            ModMenuDescPanel.configButton.GetComponent<RectTransform>().SetParent(modMenuButtonPanel.transform);
            ModMenuDescPanel.configButton.GetComponent<RectTransform>().anchorMin = new Vector2(2f / 3f, 0f);
            ModMenuDescPanel.configButton.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            ModMenuDescPanel.configButton.GetComponent<RectTransform>().offsetMin = new Vector2(10, 10);
            ModMenuDescPanel.configButton.GetComponent<RectTransform>().offsetMax = new Vector2(-10, -10);
            ModMenuDescPanel.configButton.GetComponent<Image>().sprite = BoxSprite;
            ModMenuDescPanel.configButton.GetComponent<Image>().type = Image.Type.Sliced;
            ModMenuDescPanel.configButton.GetComponent<Image>().fillCenter = true;
            ModMenuDescPanel.configButton.targetGraphic = ModMenuDescPanel.configButton.GetComponent<Image>();
            ModMenuDescPanel.configButton.onClick.AddListener(ModMenuDescPanel.ConfigButton);
            Text configButtonText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
            configButtonText.rectTransform.SetParent(ModMenuDescPanel.configButton.transform);
            configButtonText.rectTransform.anchorMin = new Vector2(0f, 0f);
            configButtonText.rectTransform.anchorMax = new Vector2(1f, 1f);
            configButtonText.rectTransform.offsetMin = Vector2.zero;
            configButtonText.rectTransform.offsetMax = Vector2.zero;
            configButtonText.alignment = TextAnchor.MiddleCenter;
            configButtonText.font = modMenuDescText.font;
            configButtonText.fontSize = 12;
            configButtonText.text = "Configure";
            ModMenuDescPanel.umfConfigButton = new GameObject("UMF Config Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).GetComponent<Button>();
            ModMenuDescPanel.umfConfigButton.GetComponent<RectTransform>().SetParent(ModMenuCanvas.transform);
            ModMenuDescPanel.umfConfigButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.025f, 0.4f);
            ModMenuDescPanel.umfConfigButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.125f, 0.6f);
            ModMenuDescPanel.umfConfigButton.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            ModMenuDescPanel.umfConfigButton.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            ModMenuDescPanel.umfConfigButton.GetComponent<Image>().sprite = BoxSprite;
            ModMenuDescPanel.umfConfigButton.GetComponent<Image>().type = Image.Type.Sliced;
            ModMenuDescPanel.umfConfigButton.GetComponent<Image>().fillCenter = true;
            ModMenuDescPanel.umfConfigButton.targetGraphic = ModMenuDescPanel.umfConfigButton.GetComponent<Image>();
            ModMenuDescPanel.umfConfigButton.onClick.AddListener(ModMenuDescPanel.UMFConfigButton);
            Text umfConfigButtonText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
            umfConfigButtonText.rectTransform.SetParent(ModMenuDescPanel.umfConfigButton.transform);
            umfConfigButtonText.rectTransform.anchorMin = new Vector2(0f, 0f);
            umfConfigButtonText.rectTransform.anchorMax = new Vector2(1f, 1f);
            umfConfigButtonText.rectTransform.offsetMin = Vector2.zero;
            umfConfigButtonText.rectTransform.offsetMax = Vector2.zero;
            umfConfigButtonText.alignment = TextAnchor.MiddleCenter;
            umfConfigButtonText.font = modMenuDescText.font;
            umfConfigButtonText.fontSize = 12;
            umfConfigButtonText.text = "Configure UMF";
            Button configReloadButton = new GameObject("Config Reload Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).GetComponent<Button>();
            configReloadButton.GetComponent<RectTransform>().SetParent(ModMenuCanvas.transform);
            configReloadButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.875f, 0.4f);
            configReloadButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.975f, 0.6f);
            configReloadButton.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            configReloadButton.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            configReloadButton.GetComponent<Image>().sprite = BoxSprite;
            configReloadButton.GetComponent<Image>().type = Image.Type.Sliced;
            configReloadButton.GetComponent<Image>().fillCenter = true;
            configReloadButton.targetGraphic = configReloadButton.GetComponent<Image>();
            configReloadButton.onClick.AddListener(() =>
            {
                foreach (string mod in UMFData.ModNames)
                {
                    UMFGUI.SendCommand("cfgReload " + mod);
                }
                GadgetModConfigs.ResetAllConfigMenus();
            });
            Text configReloadButtonText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
            configReloadButtonText.rectTransform.SetParent(configReloadButton.transform);
            configReloadButtonText.rectTransform.anchorMin = new Vector2(0f, 0f);
            configReloadButtonText.rectTransform.anchorMax = new Vector2(1f, 1f);
            configReloadButtonText.rectTransform.offsetMin = Vector2.zero;
            configReloadButtonText.rectTransform.offsetMax = Vector2.zero;
            configReloadButtonText.alignment = TextAnchor.MiddleCenter;
            configReloadButtonText.font = modMenuDescText.font;
            configReloadButtonText.fontSize = 12;
            configReloadButtonText.text = "Reload Configs";
            ModMenuDescPanel.enableUMFButton = new GameObject("Enable UMF Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).GetComponent<Button>();
            ModMenuDescPanel.enableUMFButton.GetComponent<RectTransform>().SetParent(modMenuButtonPanel.transform);
            ModMenuDescPanel.enableUMFButton.GetComponent<RectTransform>().anchorMin = new Vector2(1f / 3f, 0f);
            ModMenuDescPanel.enableUMFButton.GetComponent<RectTransform>().anchorMax = new Vector2(2f / 3f, 1f);
            ModMenuDescPanel.enableUMFButton.GetComponent<RectTransform>().offsetMin = new Vector2(10, 10);
            ModMenuDescPanel.enableUMFButton.GetComponent<RectTransform>().offsetMax = new Vector2(-10, -10);
            ModMenuDescPanel.enableUMFButton.GetComponent<Image>().sprite = BoxSprite;
            ModMenuDescPanel.enableUMFButton.GetComponent<Image>().type = Image.Type.Sliced;
            ModMenuDescPanel.enableUMFButton.GetComponent<Image>().fillCenter = true;
            ModMenuDescPanel.enableUMFButton.targetGraphic = ModMenuDescPanel.enableUMFButton.GetComponent<Image>();
            ModMenuDescPanel.enableUMFButton.onClick.AddListener(ModMenuDescPanel.EnableUMFButton);
            Text enableUMFButtonText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
            enableUMFButtonText.rectTransform.SetParent(ModMenuDescPanel.enableUMFButton.transform);
            enableUMFButtonText.rectTransform.anchorMin = new Vector2(0f, 0f);
            enableUMFButtonText.rectTransform.anchorMax = new Vector2(1f, 1f);
            enableUMFButtonText.rectTransform.offsetMin = Vector2.zero;
            enableUMFButtonText.rectTransform.offsetMax = Vector2.zero;
            enableUMFButtonText.alignment = TextAnchor.MiddleCenter;
            enableUMFButtonText.font = modMenuDescText.font;
            enableUMFButtonText.material = configButtonText.font.material;
            enableUMFButtonText.fontSize = 12;
            enableUMFButtonText.text = "Enable Mod";
            ModMenuDescPanel.enableButton = new GameObject("Enable UMF Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).GetComponent<Button>();
            ModMenuDescPanel.enableButton.GetComponent<RectTransform>().SetParent(modMenuButtonPanel.transform);
            ModMenuDescPanel.enableButton.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            ModMenuDescPanel.enableButton.GetComponent<RectTransform>().anchorMax = new Vector2(1f / 3f, 1f);
            ModMenuDescPanel.enableButton.GetComponent<RectTransform>().offsetMin = new Vector2(10, 10);
            ModMenuDescPanel.enableButton.GetComponent<RectTransform>().offsetMax = new Vector2(-10, -10);
            ModMenuDescPanel.enableButton.GetComponent<Image>().sprite = BoxSprite;
            ModMenuDescPanel.enableButton.GetComponent<Image>().type = Image.Type.Sliced;
            ModMenuDescPanel.enableButton.GetComponent<Image>().fillCenter = true;
            ModMenuDescPanel.enableButton.targetGraphic = ModMenuDescPanel.enableButton.GetComponent<Image>();
            ModMenuDescPanel.enableButton.onClick.AddListener(ModMenuDescPanel.EnableButton);
            Text enableButtonText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
            enableButtonText.rectTransform.SetParent(ModMenuDescPanel.enableButton.transform);
            enableButtonText.rectTransform.anchorMin = new Vector2(0f, 0f);
            enableButtonText.rectTransform.anchorMax = new Vector2(1f, 1f);
            enableButtonText.rectTransform.offsetMin = Vector2.zero;
            enableButtonText.rectTransform.offsetMax = Vector2.zero;
            enableButtonText.alignment = TextAnchor.MiddleCenter;
            enableButtonText.font = modMenuDescText.font;
            enableButtonText.fontSize = 12;
            enableButtonText.text = "Enable Gadget";
            ModMenuDescPanel.unpackButton = new GameObject("Unpack Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button)).GetComponent<Button>();
            ModMenuDescPanel.unpackButton.GetComponent<RectTransform>().SetParent(modMenuButtonPanel.transform);
            ModMenuDescPanel.unpackButton.GetComponent<RectTransform>().anchorMin = new Vector2(2f / 3f, 1f);
            ModMenuDescPanel.unpackButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 2f);
            ModMenuDescPanel.unpackButton.GetComponent<RectTransform>().offsetMin = new Vector2(10, 20);
            ModMenuDescPanel.unpackButton.GetComponent<RectTransform>().offsetMax = new Vector2(-10, 0);
            ModMenuDescPanel.unpackButton.GetComponent<Image>().sprite = BoxSprite;
            ModMenuDescPanel.unpackButton.GetComponent<Image>().type = Image.Type.Sliced;
            ModMenuDescPanel.unpackButton.GetComponent<Image>().fillCenter = true;
            ModMenuDescPanel.unpackButton.targetGraphic = ModMenuDescPanel.unpackButton.GetComponent<Image>();
            ModMenuDescPanel.unpackButton.onClick.AddListener(ModMenuDescPanel.UnpackButton);
            Text unpackButtonText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
            unpackButtonText.rectTransform.SetParent(ModMenuDescPanel.unpackButton.transform);
            unpackButtonText.rectTransform.anchorMin = new Vector2(0f, 0f);
            unpackButtonText.rectTransform.anchorMax = new Vector2(1f, 1f);
            unpackButtonText.rectTransform.offsetMin = Vector2.zero;
            unpackButtonText.rectTransform.offsetMax = Vector2.zero;
            unpackButtonText.alignment = TextAnchor.MiddleCenter;
            unpackButtonText.font = modMenuDescText.font;
            unpackButtonText.fontSize = 12;
            unpackButtonText.text = "Unpack Mod";

            GadgetModInfo[] gadgetMods = GadgetMods.ListAllModInfos();
            string[] allMods = gadgetMods.Select(x => x.Attribute.Name).Concat(GadgetCore.nonGadgetMods).Concat(GadgetCore.disabledMods).Concat(GadgetCore.incompatibleMods).Concat(GadgetCore.packedMods).ToArray();
            int gadgetModCount = GadgetMods.CountMods();
            int normalModCount = GadgetCore.nonGadgetMods.Count;
            int disabledModCount = GadgetCore.disabledMods.Count;
            int incompatibleModCount = GadgetCore.incompatibleMods.Count;
            ScrollRect modListScrollView = new GameObject("Scroll View", typeof(RectTransform), typeof(ScrollRect), typeof(CanvasRenderer), typeof(Image)).GetComponent<ScrollRect>();
            modListScrollView.GetComponent<RectTransform>().SetParent(ModMenuPanel);
            modListScrollView.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            modListScrollView.GetComponent<RectTransform>().anchorMax = new Vector2(0.3f, 1f);
            modListScrollView.GetComponent<RectTransform>().offsetMin = new Vector2(10, 10);
            modListScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -10);
            modListScrollView.GetComponent<Image>().sprite = BoxSprite;
            modListScrollView.GetComponent<Image>().type = Image.Type.Sliced;
            modListScrollView.GetComponent<Image>().fillCenter = true;
            Mask modListScrollViewMask = new GameObject("Mask", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Mask)).GetComponent<Mask>();
            modListScrollViewMask.GetComponent<RectTransform>().SetParent(modListScrollView.transform);
            modListScrollViewMask.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            modListScrollViewMask.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            modListScrollViewMask.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            modListScrollViewMask.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            modListScrollViewMask.GetComponent<Image>().sprite = BoxMask;
            modListScrollViewMask.GetComponent<Image>().type = Image.Type.Sliced;
            modListScrollViewMask.GetComponent<Image>().fillCenter = true;
            modListScrollViewMask.showMaskGraphic = false;
            RectTransform modListViewport = new GameObject("Viewport", typeof(RectTransform)).GetComponent<RectTransform>();
            modListViewport.SetParent(modListScrollViewMask.transform);
            modListViewport.anchorMin = new Vector2(0f, 0f);
            modListViewport.anchorMax = new Vector2(1f, 1f);
            modListViewport.offsetMin = new Vector2(10, 10);
            modListViewport.offsetMax = new Vector2(-10, -10);
            RectTransform modList = new GameObject("ModList", typeof(RectTransform), typeof(ToggleGroup)).GetComponent<RectTransform>();
            modList.SetParent(modListViewport);
            modList.anchorMin = new Vector2(0f, allMods.Length <= 6 ? 0f : (1f - (allMods.Length / 6f)));
            modList.anchorMax = new Vector2(1f, 1f);
            modList.offsetMin = Vector2.zero;
            modList.offsetMax = Vector2.zero;
            Scrollbar modListScrollBar = new GameObject("Scrollbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Scrollbar)).GetComponent<Scrollbar>();
            modListScrollBar.GetComponent<RectTransform>().SetParent(modListScrollView.transform);
            modListScrollBar.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
            modListScrollBar.GetComponent<RectTransform>().anchorMax = new Vector2(1.25f, 1f);
            modListScrollBar.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            modListScrollBar.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            modListScrollBar.GetComponent<Image>().sprite = BoxSprite;
            modListScrollBar.GetComponent<Image>().type = Image.Type.Sliced;
            modListScrollBar.GetComponent<Image>().fillCenter = true;
            RectTransform modListScrollBarHandle = new GameObject("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)).GetComponent<RectTransform>();
            modListScrollBarHandle.SetParent(modListScrollBar.transform);
            modListScrollBarHandle.anchorMin = new Vector2(0.05f, 0.05f);
            modListScrollBarHandle.anchorMax = new Vector2(0.95f, 0.95f);
            modListScrollBarHandle.offsetMin = Vector2.zero;
            modListScrollBarHandle.offsetMax = Vector2.zero;
            modListScrollBarHandle.GetComponent<Image>().sprite = BoxSprite;
            modListScrollBarHandle.GetComponent<Image>().type = Image.Type.Sliced;
            modListScrollBarHandle.GetComponent<Image>().fillCenter = true;
            modListScrollBar.targetGraphic = modListScrollBarHandle.GetComponent<Image>();
            modListScrollBar.handleRect = modListScrollBarHandle;
            modListScrollBar.direction = Scrollbar.Direction.BottomToTop;
            if (allMods.Length <= 5) modListScrollBar.interactable = false;
            modListScrollView.content = modList;
            modListScrollView.horizontal = false;
            modListScrollView.scrollSensitivity = 5;
            modListScrollView.movementType = ScrollRect.MovementType.Clamped;
            modListScrollView.viewport = modListViewport;
            modListScrollView.verticalScrollbar = modListScrollBar;
            float entryHeight = allMods.Length <= 6 ? (1f / 6f) : (1f / allMods.Length);
            Toggle firstToggle = null;
            for (int i = 0; i < allMods.Length; i++)
            {
                RectTransform modEntry = new GameObject("Mod Entry: " + allMods[i], typeof(RectTransform), typeof(Toggle), typeof(CanvasRenderer), typeof(Image), typeof(Mask)).GetComponent<RectTransform>();
                modEntry.SetParent(modList);
                modEntry.anchorMin = new Vector2(0f, 1 - ((i + 1) * entryHeight));
                modEntry.anchorMax = new Vector2(1f, 1 - (i * entryHeight));
                modEntry.offsetMin = Vector2.zero;
                modEntry.offsetMax = Vector2.zero;
                Toggle modToggle = modEntry.GetComponent<Toggle>();
                Image modSelected = new GameObject("Selected", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)).GetComponent<Image>();
                modSelected.rectTransform.SetParent(modEntry);
                modSelected.rectTransform.anchorMin = new Vector2(0f, 0f);
                modSelected.rectTransform.anchorMax = new Vector2(1f, 1f);
                modSelected.rectTransform.offsetMin = Vector2.zero;
                modSelected.rectTransform.offsetMax = Vector2.zero;
                modSelected.sprite = BoxSprite;
                modSelected.type = Image.Type.Sliced;
                modSelected.color = i < gadgetModCount ? new Color(1f, 1f, 0.25f, 1f) : i >= gadgetModCount + normalModCount + disabledModCount + incompatibleModCount ? new Color(0.5f, 0.5f, 0.5f, 1f) : i >= gadgetModCount + normalModCount + disabledModCount ? new Color(1f, 0f, 0f, 1f) : i >= gadgetModCount + normalModCount ? new Color(0.25f, 0.25f, 0.25f, 1f) : new Color(1f, 1f, 1f, 1f);
                Text modLabel = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
                modLabel.rectTransform.SetParent(modEntry);
                modLabel.rectTransform.anchorMin = new Vector2(0f, 0f);
                modLabel.rectTransform.anchorMax = new Vector2(1f, 1f);
                modLabel.rectTransform.offsetMin = new Vector2(10, 10);
                modLabel.rectTransform.offsetMax = new Vector2(-10, -10);
                modLabel.font = ModConfigMenuText.GetComponent<TextMesh>().font;
                modLabel.fontSize = 12;
                modLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
                modLabel.verticalOverflow = VerticalWrapMode.Overflow;
                modLabel.alignment = TextAnchor.MiddleLeft;
                modLabel.text = (i < gadgetModCount + normalModCount + disabledModCount + incompatibleModCount ? allMods[i] : Path.GetFileNameWithoutExtension(allMods[i])) + Environment.NewLine + (i < gadgetModCount ? ("Gadget Mod (" + GadgetMods.GetModInfo(allMods[i]).UMFName + ")") : (i < gadgetModCount + normalModCount ? "Non-Gadget Mod" : i < gadgetModCount + normalModCount + disabledModCount ? "Disabled" : i < gadgetModCount + normalModCount + disabledModCount + incompatibleModCount ? "Incompatible" : "Packed Mod"));
                if ((i < gadgetModCount && !gadgetMods[i].Mod.Enabled) || i >= gadgetModCount + GadgetCore.nonGadgetMods.Count) modLabel.color = new Color(1f, 1f, 1f, 0.5f);
                modToggle.GetComponent<Image>().sprite = BoxSprite;
                modToggle.GetComponent<Image>().type = Image.Type.Sliced;
                modToggle.GetComponent<Image>().color = i < gadgetModCount ? new Color(1f, 1f, 0.25f, 0.25f) : i >= gadgetModCount + normalModCount + disabledModCount + incompatibleModCount ? new Color(0.5f, 0.5f, 0.5f, 0.25f) : i >= gadgetModCount + normalModCount + disabledModCount ? new Color(1f, 0f, 0f, 0.25f) : i >= gadgetModCount + normalModCount ? new Color(0.25f, 0.25f, 0.25f, 0.25f) : new Color(1f, 1f, 1f, 0.25f);
                modToggle.transition = Selectable.Transition.None;
                modToggle.isOn = i == 0;
                modToggle.toggleTransition = Toggle.ToggleTransition.None;
                modToggle.graphic = modSelected;
                modToggle.group = modList.GetComponent<ToggleGroup>();
                int toggleIndex = i;
                if (i == 0) firstToggle = modToggle;
                modToggle.onValueChanged.AddListener((toggled) => { if (toggled) ModMenuDescPanel.UpdateInfo(modToggle, toggleIndex); });
            }

            if (allMods.Length > 0) ModMenuDescPanel.UpdateInfo(firstToggle, 0);
        }
    }
}

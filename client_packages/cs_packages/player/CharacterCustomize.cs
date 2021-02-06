using cs_packages.model;
using RAGE;
using RAGE.Elements;
using RAGE.NUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.player
{
    class CharacterCustomize : Events.Script
    {
        Customize model = new Customize();
        private bool menuactive = false;

        List<object> fathers = new List<object> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 42, 43, 44 };
        List<object> mothers = new List<object> { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 45 };

        //global
        List<List<object>> hairIDList = new List<List<object>>{
            // male
            new List<object>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 30, 31, 32, 33, 34, 73, 76, 77, 78, 79, 80, 81, 82, 84, 85 },
            // female
            new List<object>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 31, 76, 77, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 90, 91}
        };

        List<List<int>> validTorsoIDs = new List<List<int>>{
            new List<int>{ 0, 0, 2, 14, 14, 5, 14, 14, 8, 0, 14, 15, 12 },
            new List<int>{ 0, 5, 2, 3, 4, 4, 5, 5, 5, 0 }
        };

        int outClothes = 1;
        int pants = 0;
        int shoes = 1;

        int hair = 0;
        int hairColor = 0;
        int eyeColor = 0;

        bool gender = true;
        int father = 0;
        int mother = 21;
        float similarity = 0.5f;
        float skin = 0.5f;

        List<float> features = getFeaturesList();
        List<int> appearance = getAppearanceList();


        //Eventы для вызова с js Файла
        private CharacterCustomize()
        {
            Input.Bind(0x72, true, ShowCharacterCustomizeMenu);//f3
            Events.OnPlayerCommand += OnPlayerCommand;
            //Events.Add("changeCharacterGender", OnChangeCharacterGender);
            //Events.Add("EditorList", onEditorList);
        }

        private MenuPool menuPool;

        public void ShowCharacterCustomizeMenu()
        {
            if (menuactive) return;
            menuactive = true;
            menuPool = new MenuPool();
            UIMenu mainMenu = new UIMenu("Редактор персонажа", "123");
            menuPool.Add(mainMenu);
            //генетика
            UIMenu genethic = menuPool.AddSubMenu(mainMenu, "Генетика");
            //пол
            UIMenuListItem PolList = new UIMenuListItem("Пол", new List<object> {"М", "Ж"}, 0);
            genethic.AddItem(PolList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == PolList)
                {
                    if ((string)item.IndexToItem(index) == "М")
                    {
                        onEditorList(new object[] { "gender", true });
                    }
                    else 
                    {
                        onEditorList(new object[] { "gender", false });
                    }
                }
            };
            UIMenuListItem batyaList = new UIMenuListItem("Отец", fathers, 0);
            genethic.AddItem(batyaList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == batyaList)
                {
                    onEditorList(new object[] { "father", item.IndexToItem(index) });
                }
            };

            UIMenuListItem matyaList = new UIMenuListItem("Мать", mothers, 0);
            genethic.AddItem(matyaList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == matyaList)
                {
                    onEditorList(new object[] { "mother", item.IndexToItem(index) });
                }
            };

            UIMenuListItem simList = new UIMenuListItem("Схожесть", GetFloatList(0.0f, 1.0f, 0.1f), 0);
            genethic.AddItem(simList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == simList)
                {
                    onEditorList(new object[] { "similar", item.IndexToItem(index) });
                }
            };
            UIMenuListItem skinList = new UIMenuListItem("Цвет кожи", GetFloatList(0.0f, 1.0f, 0.1f), 0);
            genethic.AddItem(skinList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == skinList)
                {
                    onEditorList(new object[] { "skin", item.IndexToItem(index) });
                }
            };
            //Лицо
            UIMenu face = menuPool.AddSubMenu(mainMenu, "Черты лица");
            List<object> valueList= GetFloatList(-1.0f, 1.0f, 0.1f);
            UIMenuListItem noseWidth = new UIMenuListItem("Ширина носа", valueList, 0);
            face.AddItem(noseWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseWidth)
                {
                    onEditorList(new object[] { "noseWidth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem noseHeight = new UIMenuListItem("Высота носа", valueList, 0);
            face.AddItem(noseHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseHeight)
                {
                    onEditorList(new object[] { "noseHeight", item.IndexToItem(index) });
                }
            };
            UIMenuListItem noseTipLength = new UIMenuListItem("Глубина кончика носа", valueList, 0);
            face.AddItem(noseTipLength);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseTipLength)
                {
                    onEditorList(new object[] { "noseTipLength", item.IndexToItem(index) });
                }
            };
            UIMenuListItem noseDepth = new UIMenuListItem("Высота кончика носа", valueList, 0);
            face.AddItem(noseDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseDepth)
                {
                    onEditorList(new object[] { "noseDepth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem noseTipHeight = new UIMenuListItem("Выстоа кончика носа", valueList, 0);
            face.AddItem(noseTipHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseTipHeight)
                {
                    onEditorList(new object[] { "noseTipHeight", item.IndexToItem(index) });
                }
            };
            UIMenuListItem noseBroke = new UIMenuListItem("Поломанность носа", valueList, 0);
            face.AddItem(noseBroke);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    onEditorList(new object[] { "noseTipHeight", item.IndexToItem(index) });
                }
            };
            UIMenuListItem eyebrowHeight = new UIMenuListItem("Высота бровей", valueList, 0);
            face.AddItem(eyebrowHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowHeight)
                {
                    onEditorList(new object[] { "eyebrowHeight", item.IndexToItem(index) });
                }
            };
            UIMenuListItem eyebrowDepth = new UIMenuListItem("Глубина бровей", valueList, 0);
            face.AddItem(eyebrowDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowDepth)
                {
                    onEditorList(new object[] { "eyebrowDepth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem cheekboneHeight = new UIMenuListItem("Высота скул", valueList, 0);
            face.AddItem(cheekboneHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == cheekboneHeight)
                {
                    onEditorList(new object[] { "cheekboneHeight", item.IndexToItem(index) });
                }
            };
            UIMenuListItem cheekboneWidth = new UIMenuListItem("Ширина скул", valueList, 0);
            face.AddItem(cheekboneWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    onEditorList(new object[] { "cheekboneWidth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem cheekDepth = new UIMenuListItem("Глубина щеки", valueList, 0);
            face.AddItem(cheekDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    onEditorList(new object[] { "cheekDepth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem eyeScale = new UIMenuListItem("Размер глаз", valueList, 0);
            face.AddItem(eyeScale);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    onEditorList(new object[] { "eyeScale", item.IndexToItem(index) });
                }
            };
            UIMenuListItem lipThickness = new UIMenuListItem("Толщина губ", valueList, 0);
            face.AddItem(lipThickness);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == lipThickness)
                {
                    onEditorList(new object[] { "lipThickness", item.IndexToItem(index) });
                }
            };
            UIMenuListItem jawWidth = new UIMenuListItem("Ширина челюсти", valueList, 0);
            face.AddItem(jawWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == jawWidth)
                {
                    onEditorList(new object[] { "jawWidth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem jawShape = new UIMenuListItem("Форма челюсти", valueList, 0);
            face.AddItem(jawShape);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == jawShape)
                {
                    onEditorList(new object[] { "jawShape", item.IndexToItem(index) });
                }
            };
            UIMenuListItem chinHeight = new UIMenuListItem("Высота подбородка", valueList, 0);
            face.AddItem(chinHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinHeight)
                {
                    onEditorList(new object[] { "chinHeight", item.IndexToItem(index) });
                }
            };
            UIMenuListItem chinDepth = new UIMenuListItem("Глубина подбородка", valueList, 0);
            face.AddItem(chinDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinDepth)
                {
                    onEditorList(new object[] { "chinDepth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem chinWidth = new UIMenuListItem("Ширина подбородка", valueList, 0);
            face.AddItem(chinWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinWidth)
                {
                    onEditorList(new object[] { "chinWidth", item.IndexToItem(index) });
                }
            };
            UIMenuListItem chinIndent = new UIMenuListItem("Отступ подбородка", valueList, 0);
            face.AddItem(chinIndent);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinIndent)
                {
                    onEditorList(new object[] { "chinIndent", item.IndexToItem(index) });
                }
            };
            UIMenuListItem neck = new UIMenuListItem("Шея", valueList, 0);
            face.AddItem(neck);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == neck)
                {
                    onEditorList(new object[] { "neck", item.IndexToItem(index) });
                }
            };
            ////Внешность
            UIMenu apper = menuPool.AddSubMenu(mainMenu, "Внешность");
            List<object> MHair = hairIDList[0];            
            UIMenuListItem hairM = new UIMenuListItem("Причёска М", MHair, 0);
            apper.AddItem(hairM);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == hairM)
                {
                    onEditorList(new object[] { "hair", item.IndexToItem(index) });
                }
            };
            List<object> FHair = hairIDList[1];
            UIMenuListItem hairF = new UIMenuListItem("Причёска Ж", MHair, 0);
            apper.AddItem(hairF);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == hairF)
                {
                    onEditorList(new object[] { "hair", item.IndexToItem(index) });
                }
            };
            UIMenuListItem eyebrowsM = new UIMenuListItem("Брови М", GetIntList(0, 33, 1), 0);
            apper.AddItem(eyebrowsM);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowsM)
                {
                    Chat.Output(item.IndexToItem(index).GetType().Name);
                    onEditorList(new object[] { "eyebrows", item.IndexToItem(index) });
                }
            };
            UIMenuListItem eyebrowsF = new UIMenuListItem("Брови Ж", GetIntList(0, 33, 1), 0);
            apper.AddItem(eyebrowsF);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowsF)
                {
                    onEditorList(new object[] { "eyebrows", item.IndexToItem(index) });
                }
            };
            UIMenuListItem beard = new UIMenuListItem("Борода", GetIntList(0, 28, 1), 0);
            apper.AddItem(beard);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == beard)
                {
                    onEditorList(new object[] { "beard", item.IndexToItem(index) });
                }
            };
            UIMenuListItem hairColor = new UIMenuListItem("Цвет волос", GetIntList(0, 10, 1), 0);
            apper.AddItem(hairColor);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == hairColor)
                {
                    onEditorList(new object[] { "hairColor", item.IndexToItem(index) });
                }
            };
            UIMenuListItem eyeColor = new UIMenuListItem("Цвет глаз", GetIntList(1, 10, 1), 0);
            apper.AddItem(eyeColor);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == eyeColor)
                {
                    onEditorList(new object[] { "eyeColor", item.IndexToItem(index) });
                }
            };
            UIMenuItem save = new UIMenuItem("Сохранить");
            mainMenu.AddItem(save);
            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == save)
                {
                    OnSaveCharacter();
                }
            };
            //face.RemoveItemAt(hair.Index);

            menuPool.RefreshIndex();
            Events.Tick += DrawMenu;
            mainMenu.Visible = true;
            mainMenu.OnMenuClose += (sender) =>
            {
                menuactive = false;
                Chat.Output("ЗАКРЫТО!");
                Events.Tick -= DrawMenu;
            };
        }

        private void DrawMenu(List<Events.TickNametagData> nametags)
        {
            if (menuactive)
            {
                menuPool.ProcessMenus();
            }

        }

        public void onEditorList(object[] args)
        {
            string field = args[0].ToString();
            object val = args[1];
            Chat.Output(field);
            Chat.Output(val.ToString());
            Chat.Output(val.GetType().Name);



            model.GetType().GetProperty(field).SetValue(model, val);
            Update(field);
        }

        public void UpdateCharacterParents()
        {
            Player.LocalPlayer.SetHeadBlendData(
                model.mother,
                model.father,
                0,

                model.mother,
                model.father,
                0,

                model.similar,
                model.skin,
                0.0f,

                true
            );
        }
        public void UpdateCharacterHair()
        {
            // hair
            Player.LocalPlayer.SetComponentVariation(2, model.hair, 0, 0);
            Player.LocalPlayer.SetHairColor(model.hairColor, 0);

            // appearance colors
            Player.LocalPlayer.SetHeadOverlayColor(2, 1, model.hairColor, 100); // eyebrow
            Player.LocalPlayer.SetHeadOverlayColor(1, 1, model.hairColor, 100); // beard
            Player.LocalPlayer.SetHeadOverlayColor(10, 1, model.hairColor, 100); // chesthair

            // eye color
            Player.LocalPlayer.SetEyeColor(model.eyeColor);
        }
        public void Update(string f)
        {
            switch (f)
            {
                case "gender":
                    OnChangeCharacterGender();
                    return;
                case "similar":
                    UpdateCharacterParents();
                    return;
                case "skin":
                    UpdateCharacterParents();
                    return;
                case "father":
                    UpdateCharacterParents();
                    return;
                case "mother":
                    UpdateCharacterParents();
                    return;
                case "hair":
                    UpdateCharacterHair();
                    return;
                case "eyebrows": Player.LocalPlayer.SetHeadOverlay(2, (int)model.GetType().GetProperty(f).GetValue(model), 100); return;
                case "beard": Player.LocalPlayer.SetHeadOverlay(1, (int)model.GetType().GetProperty(f).GetValue(model), 100); return;
                case "hairColor":
                    UpdateCharacterHair();
                    return;
                case "eyeColor":
                    UpdateCharacterHair();
                    return;
                case "noseWidth": Player.LocalPlayer.SetFaceFeature(0, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "noseHeight": Player.LocalPlayer.SetFaceFeature(1, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "noseTipLength": Player.LocalPlayer.SetFaceFeature(2, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "noseDepth": Player.LocalPlayer.SetFaceFeature(3, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "noseTipHeight": Player.LocalPlayer.SetFaceFeature(4, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "noseBroke": Player.LocalPlayer.SetFaceFeature(5, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "eyebrowHeight": Player.LocalPlayer.SetFaceFeature(6, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "eyebrowDepth": Player.LocalPlayer.SetFaceFeature(7, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "cheekboneHeight": Player.LocalPlayer.SetFaceFeature(8, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "cheekboneWidth": Player.LocalPlayer.SetFaceFeature(9, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "cheekDepth": Player.LocalPlayer.SetFaceFeature(10, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "eyeScale": Player.LocalPlayer.SetFaceFeature(11, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "lipThickness": Player.LocalPlayer.SetFaceFeature(12, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "jawWidth": Player.LocalPlayer.SetFaceFeature(13, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "jawShape": Player.LocalPlayer.SetFaceFeature(14, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "chinHeight": Player.LocalPlayer.SetFaceFeature(15, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "chinDepth": Player.LocalPlayer.SetFaceFeature(16, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "chinWidth": Player.LocalPlayer.SetFaceFeature(17, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "chinIndent": Player.LocalPlayer.SetFaceFeature(18, (float)model.GetType().GetProperty(f).GetValue(model)); return;
                case "neck": Player.LocalPlayer.SetFaceFeature(19, (float)model.GetType().GetProperty(f).GetValue(model)); return;
            }
        }

        public void OnChangeCharacterGender()
        {
            if (model.gender)
            {
                Player.LocalPlayer.Model = RAGE.Game.Misc.GetHashKey("mp_m_freemode_01");
                outClothes = 1;
                pants = 0;
                shoes = 1;
            }
            else
            {
                Player.LocalPlayer.Model = RAGE.Game.Misc.GetHashKey("mp_f_freemode_01");
                outClothes = 5;
                pants = 0;
                shoes = 3;
            }

            appearance[1] = 255;

            UpdateAllModel();
            UpdateClothes();

        }

        public void UpdateClothes()
        {
            Player.LocalPlayer.SetComponentVariation(11, outClothes, 1, 0);
            Player.LocalPlayer.SetComponentVariation(4, pants, 1, 0);
            Player.LocalPlayer.SetComponentVariation(6, shoes, 1, 0);
            Player.LocalPlayer.SetComponentVariation(8, 15, 0, 0);
            int currentGender = (model.gender) ? 0 : 1;
            Player.LocalPlayer.SetComponentVariation(3, validTorsoIDs[currentGender][outClothes], 0, 0);
        }

        public void OnSaveCharacter()
        {
            Chat.Output("Сохранение идёт");
            Events.CallRemote("remote_SaveCustomization", model);

            Chat.Output("Сохранение завершено");
        }

        private static List<float> getFeaturesList()
        {
            List<float> list = new List<float>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(0.0f);
            }
            return list;
        }

        private static List<int> getAppearanceList()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 11; i++)
            {
                list.Add(255);
            }
            return list;
        }

        public void UpdateAllModel() 
        {
            foreach (var obj in model.GetType().GetProperties())
            {
                if(obj.Name != "gender")
                {
                    Update(obj.Name);
                }
                //Chat.Output(obj.Name);
            }
        }

        private List<object> GetFloatList(float begin, float end, float step)
        {
            List<object> floatList = new List<object>();
            for (float i = begin; i <= end; i += step)
            {
                floatList.Add(i);
            }
            return floatList;
        }

        private List<object> GetIntList(int begin, int end, int step)
        {
            List<object> list = new List<object>();
            for (int i = begin; i <= end; i += step)
            {
                list.Add(i);
            }
            return list;
        }

        public void OnPlayerCommand(string cmd, Events.CancelEventArgs cancel)//todo убрать
        {
            string[] args = cmd.Split(new char[] { ' ' });
            string commandName = args[0].Trim(new char[] { '/' });
            if (commandName == "tp1") 
            {
                Player.LocalPlayer.Position = new Vector3(402.5164f, -1002.847f, -99.2587f);
                return;
            }
            if (commandName == "tp2") 
            {
                Player.LocalPlayer.Position = new Vector3(-1288f, 440.748f, 97.69459f);
                return;
            }
            if (commandName == "tp3") 
            {
                Player.LocalPlayer.Position = new Vector3(152.2605f, -1004.471f, -98.99999f);
                return;
            }
            if (commandName == "tp4") 
            {
                Player.LocalPlayer.Position = new Vector3(520.0f, 4750.0f, -70.0f);
                return;
            }
            if (commandName == "tp5") 
            {
                Player.LocalPlayer.Position = new Vector3(-1421.015f, -3012.587f, -80.000f);
                return;
            }
            if (commandName == "tp6") 
            {
                Player.LocalPlayer.Position = new Vector3(2147.91f, 2921.0f, -61.9f);
                return;
            }
            if (commandName == "tp7") 
            {
                Player.LocalPlayer.Position = new Vector3(2168.0f, 2920.0f, -84.0f);
                return;
            }
            if (commandName == "tp8") 
            {
                Player.LocalPlayer.Position = new Vector3(345.0041f, 4842.001f, -59.9997f);
                return;
            }
            if (commandName == "tp9") 
            {
                Player.LocalPlayer.Position = new Vector3(117.22f, -620.938f, 206.1398f);
                return;
            }
            if (commandName == "tp10") 
            {
                Player.LocalPlayer.Position = new Vector3(-1044.193f, -236.9535f, 37.96496f);
                return;
            }
            if (commandName == "tp11") 
            {
                Player.LocalPlayer.Position = new Vector3(1399f, 1150f, 115f);
                return;
            }
            if (commandName == "tp12") 
            {
                Player.LocalPlayer.Position = new Vector3(2331.344f, 2574.073f, 46.68137f);
                return;
            }

        }
    }
}

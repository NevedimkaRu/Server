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
                    Chat.Output("Услышан");
                    if ((string)item.IndexToItem(index) == "М")
                    {
                        gender = true;
                    }
                    else {
                        gender = false;
                    }
                    OnChangeCharacterGender();
                }
            };

            UIMenuListItem batyaList = new UIMenuListItem("Отец", fathers, 0);
            genethic.AddItem(batyaList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == batyaList)
                {
                    father = (int)item.IndexToItem(index);
                    UpdateCharacterParents();
                }
            };

            UIMenuListItem matyaList = new UIMenuListItem("Мать", mothers, 0);
            genethic.AddItem(matyaList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == matyaList)
                {
                    mother = (int)item.IndexToItem(index);
                    UpdateCharacterParents();
                }
            };

            UIMenuListItem simList = new UIMenuListItem("Схожесть", GetFloatList(0.0f, 1.0f, 0.1f), 0);
            genethic.AddItem(simList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == simList)
                {
                    similarity = (float)item.IndexToItem(index);
                    UpdateCharacterParents();
                }
            };
            UIMenuListItem skinList = new UIMenuListItem("Цвет кожи", GetFloatList(0.0f, 1.0f, 0.1f), 0);
            genethic.AddItem(skinList);
            genethic.OnListChange += (sender, item, index) =>
            {
                if (item == skinList)
                {
                    skin = (float)item.IndexToItem(index);
                    UpdateCharacterParents();
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
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "noseWidth", val });
                }
            };
            UIMenuListItem noseHeight = new UIMenuListItem("Высота носа", valueList, 0);
            face.AddItem(noseHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseHeight)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "noseHeight", val });
                }
            };
            UIMenuListItem noseTipLength = new UIMenuListItem("Глубина кончика носа", valueList, 0);
            face.AddItem(noseTipLength);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseTipLength)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "noseTipLength", val });
                }
            };
            UIMenuListItem noseDepth = new UIMenuListItem("Высота кончика носа", valueList, 0);
            face.AddItem(noseDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseDepth)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "noseDepth", val });
                }
            };
            UIMenuListItem noseTipHeight = new UIMenuListItem("Выстоа кончика носа", valueList, 0);
            face.AddItem(noseTipHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseTipHeight)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "noseTipHeight", val });
                }
            };
            UIMenuListItem noseBroke = new UIMenuListItem("Поломанность носа", valueList, 0);
            face.AddItem(noseBroke);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "noseTipHeight", val });
                }
            };
            UIMenuListItem eyebrowHeight = new UIMenuListItem("Высота бровей", valueList, 0);
            face.AddItem(eyebrowHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowHeight)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "eyebrowHeight", val });
                }
            };
            UIMenuListItem eyebrowDepth = new UIMenuListItem("Глубина бровей", valueList, 0);
            face.AddItem(eyebrowDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowDepth)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "eyebrowDepth", val });
                }
            };
            UIMenuListItem cheekboneHeight = new UIMenuListItem("Высота скул", valueList, 0);
            face.AddItem(cheekboneHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == cheekboneHeight)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "cheekboneHeight", val });
                }
            };
            UIMenuListItem cheekboneWidth = new UIMenuListItem("Ширина скул", valueList, 0);
            face.AddItem(cheekboneWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "cheekboneWidth", val });
                }
            };
            UIMenuListItem cheekDepth = new UIMenuListItem("Глубина щеки", valueList, 0);
            face.AddItem(cheekDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "cheekDepth", val });
                }
            };
            UIMenuListItem eyeScale = new UIMenuListItem("Размер глаз", valueList, 0);
            face.AddItem(eyeScale);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == noseBroke)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "eyeScale", val });
                }
            };
            UIMenuListItem lipThickness = new UIMenuListItem("Толщина губ", valueList, 0);
            face.AddItem(lipThickness);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == lipThickness)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "lipThickness", val });
                }
            };
            UIMenuListItem jawWidth = new UIMenuListItem("Ширина челюсти", valueList, 0);
            face.AddItem(jawWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == jawWidth)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "jawWidth", val });
                }
            };
            UIMenuListItem jawShape = new UIMenuListItem("Форма челюсти", valueList, 0);
            face.AddItem(jawShape);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == jawShape)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "jawShape", val });
                }
            };
            UIMenuListItem chinHeight = new UIMenuListItem("Высота подбородка", valueList, 0);
            face.AddItem(chinHeight);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinHeight)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "chinHeight", val });
                }
            };
            UIMenuListItem chinDepth = new UIMenuListItem("Глубина подбородка", valueList, 0);
            face.AddItem(chinDepth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinDepth)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "chinDepth", val });
                }
            };
            UIMenuListItem chinWidth = new UIMenuListItem("Ширина подбородка", valueList, 0);
            face.AddItem(chinWidth);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinWidth)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "chinWidth", val });
                }
            };
            UIMenuListItem chinIndent = new UIMenuListItem("Отступ подбородка", valueList, 0);
            face.AddItem(chinIndent);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == chinIndent)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "chinIndent", val });
                }
            };
            UIMenuListItem neck = new UIMenuListItem("Шея", valueList, 0);
            face.AddItem(neck);
            face.OnListChange += (sender, item, index) =>
            {
                if (item == neck)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "neck", val });
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
                    Chat.Output(index.ToString());
                    Chat.Output(item.IndexToItem(index).ToString());
                    float val = (float) ((int) item.IndexToItem(index));
                    onEditorList(new object[] { "hairM", val });
                }
            };
            List<object> FHair = hairIDList[1];
            UIMenuListItem hairF = new UIMenuListItem("Причёска Ж", MHair, 0);
            apper.AddItem(hairF);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == hairF)
                {
                    float val = (float)((int)item.IndexToItem(index));
                    onEditorList(new object[] { "hairF", val });
                }
            };
            UIMenuListItem eyebrowsM = new UIMenuListItem("Брови М", GetFloatList(0f, 33f, 1f), 0);
            apper.AddItem(eyebrowsM);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowsM)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "eyebrowsM", val });
                }
            };
            UIMenuListItem eyebrowsF = new UIMenuListItem("Брови Ж", GetFloatList(0f, 33f, 1f), 0);
            apper.AddItem(eyebrowsF);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == eyebrowsF)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "eyebrowsF", val });
                }
            };
            UIMenuListItem beard = new UIMenuListItem("Борода", GetFloatList(0f, 28f, 1f), 0);
            apper.AddItem(beard);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == beard)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "beard", val });
                }
            };
            UIMenuListItem hairColor = new UIMenuListItem("Цвет волос", GetFloatList(0f, 10f, 1f), 0);
            apper.AddItem(hairColor);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == hairColor)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "hairColor", val });
                }
            };
            UIMenuListItem eyeColor = new UIMenuListItem("Цвет глаз", GetFloatList(1f, 10f, 1f), 0);
            apper.AddItem(eyeColor);
            apper.OnListChange += (sender, item, index) =>
            {
                if (item == eyeColor)
                {
                    float val = (float)item.IndexToItem(index);
                    onEditorList(new object[] { "eyeColor", val });
                }
            };
            UIMenu save = menuPool.AddSubMenu(mainMenu, "Сохранить");
            mainMenu.OnItemSelect += (sender, item, index) =>
            {
                OnSaveCharacter();
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

        private void OnChangeCharacterGender()
        {
            string n = "Female";
            if (gender) {
                n = "Male";
            }
            OnChangeCharacterGender(new object[] { n });
        }
        public void OnChangeCharacterGender(object[] args)
        {
            string param = args[0].ToString();
            gender = (param == "Male") ? true : false;
            if (gender)
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

            UpdateCharacterParents();
            UpdateCharacterHair();
            UpdateAppearance();
            UpdateClothes();
            for (var i = 0; i < 20; i++)
            {
                Player.LocalPlayer.SetFaceFeature(i, features[i]);
            }
        }


        public void onEditorList1(object[] args)
        {
            float lvl = (float)args[1];
            //object lvl = args[1];

            string param = args[0].ToString();

            model.GetType().GetProperty(param).SetValue(model, lvl);


            switch (param)
            {
                case "similar":
                    similarity = lvl;
                    UpdateCharacterParents();
                    return;
                case "skin":
                    skin = lvl;
                    UpdateCharacterParents();
                    return;
                case "noseWidth": Player.LocalPlayer.SetFaceFeature(0, lvl); features[0] = lvl; return;
                case "noseHeight": Player.LocalPlayer.SetFaceFeature(1, lvl); features[1] = lvl; return;
                case "noseTipLength": Player.LocalPlayer.SetFaceFeature(2, lvl); features[2] = lvl; return;
                case "noseDepth": Player.LocalPlayer.SetFaceFeature(3, lvl); features[3] = lvl; return;
                case "noseTipHeight": Player.LocalPlayer.SetFaceFeature(4, lvl); features[4] = lvl; return;
                case "noseBroke": Player.LocalPlayer.SetFaceFeature(5, lvl); features[5] = lvl; return;
                case "eyebrowHeight": Player.LocalPlayer.SetFaceFeature(6, lvl); features[6] = lvl; return;
                case "eyebrowDepth": Player.LocalPlayer.SetFaceFeature(7, lvl); features[7] = lvl; return;
                case "cheekboneHeight": Player.LocalPlayer.SetFaceFeature(8, lvl); features[8] = lvl; return;
                case "cheekboneWidth": Player.LocalPlayer.SetFaceFeature(9, lvl); features[9] = lvl; return;
                case "cheekDepth": Player.LocalPlayer.SetFaceFeature(10, lvl); features[10] = lvl; return;
                case "eyeScale": Player.LocalPlayer.SetFaceFeature(11, lvl); features[11] = lvl; return;
                case "lipThickness": Player.LocalPlayer.SetFaceFeature(12, lvl); features[12] = lvl; return;
                case "jawWidth": Player.LocalPlayer.SetFaceFeature(13, lvl); features[13] = lvl; return;
                case "jawShape": Player.LocalPlayer.SetFaceFeature(14, lvl); features[14] = lvl; return;
                case "chinHeight": Player.LocalPlayer.SetFaceFeature(15, lvl); features[15] = lvl; return;
                case "chinDepth": Player.LocalPlayer.SetFaceFeature(16, lvl); features[16] = lvl; return;
                case "chinWidth": Player.LocalPlayer.SetFaceFeature(17, lvl); features[17] = lvl; return;
                case "chinIndent": Player.LocalPlayer.SetFaceFeature(18, lvl); features[18] = lvl; return;
                case "neck": Player.LocalPlayer.SetFaceFeature(19, lvl); features[19] = lvl; return;
                case "father":
                    father = (int) fathers[Convert.ToInt32(lvl)];
                    UpdateCharacterParents();
                    return;
                case "mother":
                    mother = (int )mothers[Convert.ToInt32(lvl)];
                    UpdateCharacterParents();
                    return;
                case "hairM":
                    hair = Convert.ToInt32(lvl);
                    UpdateCharacterHair();
                    return;
                case "hairF":
                    hair = Convert.ToInt32(lvl);
                    UpdateCharacterHair();
                    return;
                case "eyebrowsM":
                    appearance[2] = Convert.ToInt32(lvl);
                    UpdateAppearance();
                    return;
                case "eyebrowsF":
                    appearance[2] = Convert.ToInt32(lvl);
                    UpdateAppearance();
                    return;
                case "beard":
                    var overlay = (Convert.ToInt32(lvl) == 0) ? 255 : Convert.ToInt32(lvl) - 1;
                    appearance[1] = Convert.ToInt32(lvl);
                    UpdateAppearance();
                    return;
                case "hairColor":
                    hairColor = Convert.ToInt32(lvl);
                    UpdateCharacterHair();
                    return;
                case "eyeColor":
                    eyeColor = Convert.ToInt32(lvl);
                    UpdateCharacterHair();
                    return;
            }
        }

        public void OnSaveCharacter()
        {
            ////todo проверка на спам
            ////if (new Date().getTime() - global.lastCheck < 1000) return;
            ////global.lastCheck = new Date().getTime();
            //if (editorBrowser != null)
            //{
            //    editorBrowser.destroy();
            //    editorBrowser = null;
            //    mp.game.graphics.startScreenEffect("MinigameTransitionIn", 0, false);
            //    int currentGender = (gender) ? 0 : 1;

            //    var appearance_values = [];
            //    for (var i = 0; i < 11; i++) appearance_values.push({ Value: appearance[i], Opacity: 100 });

            //    var hair_or_colors = [];
            //    hair_or_colors.push(hairIDList[currentGender][hair]);
            //    hair_or_colors.push(hairColor);
            //    hair_or_colors.push(0);
            //    hair_or_colors.push(hairColor);
            //    hair_or_colors.push(hairColor);
            //    hair_or_colors.push(eyeColor);
            //    hair_or_colors.push(0);
            //    hair_or_colors.push(0);
            //    hair_or_colors.push(hairColor);

            //    setTimeout(function() {
            //        mp.events.callRemote("SaveCharacter", currentGender, father, mother, similarity, skin, JSON.stringify(features), JSON.stringify(appearance_values), JSON.stringify(hair_or_colors));
            //    }, 5000);
            //}
        }

        public void UpdateCharacterHair()
        {
            int currentGender = (gender) ? 0 : 1;
            // hair
            Player.LocalPlayer.SetComponentVariation(2, (int) hairIDList[currentGender][hair], 0, 0);
            Player.LocalPlayer.SetHairColor(hairColor, 0);

            // appearance colors
            Player.LocalPlayer.SetHeadOverlayColor(2, 1, hairColor, 100); // eyebrow
            Player.LocalPlayer.SetHeadOverlayColor(1, 1, hairColor, 100); // beard
            Player.LocalPlayer.SetHeadOverlayColor(10, 1, hairColor, 100); // chesthair

            // eye color
            Player.LocalPlayer.SetEyeColor(eyeColor);

            
        }

        public void UpdateCharacterParents()
        {
            Player.LocalPlayer.SetHeadBlendData(
                mother,
                father,
                0,

                mother,
                father,
                0,

                similarity,
                skin,
                0.0f,

                true
            );
        }

        public void UpdateAppearance()
        {
            for (var i = 0; i < 11; i++)
            {
                Player.LocalPlayer.SetHeadOverlay(i, appearance[i], 100);
            }
        }

        public void UpdateClothes()
        {
            Player.LocalPlayer.SetComponentVariation(11, outClothes, 1, 0);
            Player.LocalPlayer.SetComponentVariation(4, pants, 1, 0);
            Player.LocalPlayer.SetComponentVariation(6, shoes, 1, 0);
            Player.LocalPlayer.SetComponentVariation(8, 15, 0, 0);
            int currentGender = (gender) ? 0 : 1;
            Player.LocalPlayer.SetComponentVariation(3, validTorsoIDs[currentGender][outClothes], 0, 0);
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


        private List<object> GetFloatList(float begin, float end, float step)
        {
            List<object> floatList = new List<object>();
            for (float i = begin; i <= end; i += step)
            {
                floatList.Add(float.Parse(i.ToString("0.00")));
            }
            return floatList;
        }

        public void OnPlayerCommand(string cmd, Events.CancelEventArgs cancel)
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

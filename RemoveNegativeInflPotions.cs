using QATool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLib;

namespace QATool.Tests
{
    public class RemoveNegativeInflPotions : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.EnterGame(client, 0);

            Wait(() => client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)"), 10);
            client.EnterConsoleCheat("/getitem 122 3"); // панацея
            client.EnterConsoleCheat("/level 10", "/goldadd 100000", "/emerald 100000");

            //идем покупать зелья!
            client.WaitShowElementGUIAndMouseClick("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)", 3); // арсенал
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("LeftMenuButtonFramePW3(Clone)", 3, 3); // зелья
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ItemFramePW3(Clone)", 16, 3); // панацея
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("PotionSlotsHolder.ItemFramePW3(Clone)", 0, 3); // 1й слот в поясе
            Wait(1);
            BuyPotion(client, 2, 17, 1); // противоядие
            BuyPotion(client, 2, 18, 2); // охлаждения
            BuyPotion(client, 2, 19, 3); // зелье свободы действий

            client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1); // назад
            Wait(1);

            client.GoToTestLevel();
            client.EnterCheat("/statadd 8225 HpAdd", "/statadd -150 HpRegenAdd", "/hp 1");

            //используем зелья и смотрим как они работают

            CheckRemoveBuff(client, new string[] { "/infl 6 100 -1" }, 0, 300); // змедление
            CheckRemoveBuff(client, new string[] { "/infl 71 100 1" }, 1, 300); // горение
            CheckRemoveBuff(client, new string[] { "/infl 80 100 1" }, 2, 300); // отравление
            CheckRemoveBuff(client, new string[] { "/infl 6 100 -1", "/infl 71 100 1", "/infl 80 100 1", "/infl 9 100 1" }, 3, 500); // змедление, горение, отравление, безмолвие

          

            //используем зелья и смотрим что количество уменьшилось на 1
            int oldFourthPotionCount = GetPotionCount(client, 3);
            int oldThirdPotionCount = GetPotionCount(client, 2);
            int oldSecondPotionCount = GetPotionCount(client, 1);
            int oldFirstPotionCount = GetPotionCount(client, 0);

            UsePotionBySlotId(client, 3);
            UsePotionBySlotId(client, 2);
            UsePotionBySlotId(client, 1);

            bool FourthPotionCheck = oldFourthPotionCount - GetPotionCount(client, 3) == 1;
            bool ThirdPotionCheck = oldThirdPotionCount - GetPotionCount(client, 2) == 1;
            bool SecondPotionCheck = oldSecondPotionCount - GetPotionCount(client, 1) == 1;
            bool FirstPotionCheck = oldFirstPotionCount - GetPotionCount(client, 0) == 1;


            ExitToLobby(client);

        }


    }
}



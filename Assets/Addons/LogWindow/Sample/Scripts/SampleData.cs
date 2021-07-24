
namespace Bopper
{
	static public class BopperData
	{
		static public Command[] commands = new Command[] {

			new CommandPhase(0, "Setup - Introductions"),
			new CommandSay(2, "CEASE AND DESIST: LEGAL BOTS DEPLOYED alpha beta delta epsilon alpha beta"),
			new CommandSay(1, "Be gone, scoundrel!"),

			new CommandPhase(0, "Setup - Deployment"),
			new CommandDeploy(2, UnitType.BPCPI, 1205),
			new CommandDeploy(1, UnitType.BPCPI, 1920),
			new CommandDeploy(1, UnitType.JB, 1921),
			new CommandDeploy(1, UnitType.JB, 1922),
			new CommandDeploy(1, UnitType.JB, 2323),
			new CommandDeploy(1, UnitType.TB, 2425),
			new CommandDeploy(1, UnitType.TB,2425),
			new CommandDeploy(1, UnitType.LB, 1912),
			new CommandDeploy(1, UnitType.BB, 1913),
			new CommandDeploy(1, UnitType.BB, 1913),
			new CommandDeploy(2, UnitType.JB, 1221),
			new CommandDeploy(2, UnitType.JB, 1222),
			new CommandDeploy(2, UnitType.JB, 1223),
			new CommandDeploy(2, UnitType.TB, 1325),
			new CommandDeploy(2, UnitType.TB, 1325),
			new CommandDeploy(2, UnitType.LB, 1112),
			new CommandDeploy(2, UnitType.BB, 1113),
			new CommandDeploy(2, UnitType.BB, 1113),

			new CommandPhase(0, "Turn 1 - Movement"),
			new CommandMove(1, "JB-1", 1923),
			new CommandMove(1, "JB-2", 1925),
			new CommandMove(1, "JB-3", 1925),
			new CommandMove(1, "BB-1", 1915),
			new CommandMove(1, "BB-1", 1915),
			new CommandSay(1, "You can run but you cannot hide, scoundrel!"),
			new CommandMove(2, "JB-1", 1223),
			new CommandMove(2, "JB-2", 1422),
			new CommandMove(2, "JB-3", 1225),
			new CommandMove(2, "TB-1", 1328),
			new CommandMove(2, "TB-2", 1328),
			new CommandMove(2, "LB-1", 1329),
			new CommandMove(2, "BB-1", 1213),
			new CommandMove(2, "BB-2", 1213),
			new CommandSay(2, "This is a really long message that takes more than two lines and needs to expand to accomodate all content"),
			};

		static public string commandset =
@"0 PHASE Notice: commandset
0 PHASE Setup - Introductions
2 SAY CEASE AND DESIST: LEGAL BOTS DEPLOYED alpha beta delta epsilon alpha beta
1 SAY Be gone, scoundrel!
1 NAME Bell Industries
1 SHORTNAME BELL
2 NAME Weyland - Yutani
2 SHORTNAME WY
0 PHASE Setup - Deployment
2 DEPLOY BCPC 1205
1 DEPLOY BCPC 1920
1 DEPLOY JB 1921
1 DEPLOY JB 1922
1 DEPLOY JB 2323
1 DEPLOY TB 2425
1 DEPLOY TB 2425
1 DEPLOY LB 1912
1 DEPLOY BB 1913
1 DEPLOY BB 1913
2 DEPLOY JB 1221
2 DEPLOY JB 1222
2 DEPLOY JB 1223
2 DEPLOY TB 1325
2 DEPLOY TB 1325
2 DEPLOY LB 1112
2 DEPLOY BB 1113
2 DEPLOY BB 1113
0 PHASE Turn 1 - Movement
1 MOVE JB-1 1923
1 MOVE JB-2 1925
1 MOVE JB-3 1925
1 MOVE BB-1 1915
1 MOVE BB-1 1915
1 SAY You can run but you cannot hide, scoundrel!
2 MOVE JB-1 1223
2 MOVE JB-2 1422
2 MOVE JB-3 1225
2 MOVE TB-1 1328
2 MOVE TB-2 1328
2 MOVE LB-1 1329
2 MOVE BB-1 1213
2 MOVE BB-2 1213
2 SAY This is a really long message that takes more than two lines and needs to expand to accomodate all content
";

	}

	/*
	static public class BopperData
	{
		static public LogItem[] logItems = new LogItem[] {
			new PhaseLogItem(0, "Setup - Introductions"),
			new ChatLogItem(2, "CEASE AND DESIST: LEGAL BOTS DEPLOYED alpha beta delta epsilon alpha beta"),
			new ChatLogItem(1, "Be gone, scoundrel!"),
			new PhaseLogItem(0, "Setup - Deployment"),
			new CommandLogItem(2, "Deploy BCPC in hex [1205]"),
			new CommandLogItem(1, "Deploy BCPC in hex [1920]"),
			new CommandLogItem(1, "Deploy JB-1 in hex [1921]"),
			new CommandLogItem(1, "Deploy JB-2 in hex [1922]"),
			new CommandLogItem(1, "Deploy JB-3 in hex [2323]"),
			new CommandLogItem(1, "Deploy TB-1 in hex [2425]"),
			new CommandLogItem(1, "Deploy TB-2 in hex [2425]"),
			new CommandLogItem(1, "Deploy LB-1 in hex [1912]"),
			new CommandLogItem(1, "Deploy BB-1 in hex [1913]"),
			new CommandLogItem(1, "Deploy BB-1 in hex [1913]"),
			new CommandLogItem(2, "Deploy JB-1 in hex [1221]"),
			new CommandLogItem(2, "Deploy JB-2 in hex [1222]"),
			new CommandLogItem(2, "Deploy JB-3 in hex [1223]"),
			new CommandLogItem(2, "Deploy TB-1 in hex [1325]"),
			new CommandLogItem(2, "Deploy TB-2 in hex [1325]"),
			new CommandLogItem(2, "Deploy LB-1 in hex [1112]"),
			new CommandLogItem(2, "Deploy BB-1 in hex [1113]"),
			new CommandLogItem(2, "Deploy BB-1 in hex [1113]"),
			new PhaseLogItem(0, "Turn 1 - Movement"),
			new CommandLogItem(1, "Move JB-1 to hex [1923]"),
			new CommandLogItem(1, "Move JB-2 to hex [1925]"),
			new CommandLogItem(1, "Move JB-3 to hex [1925]"),
			new CommandLogItem(1, "Move BB-1 to hex [1915]"),
			new CommandLogItem(1, "Move BB-1 to hex [1915]"),
			new ChatLogItem(1, "You can run but you cannot hide, scoundrel!"),
			new CommandLogItem(2, "Move JB-1 to hex [1223]"),
			new CommandLogItem(2, "Move JB-2 to hex [1422]"),
			new CommandLogItem(2, "Move JB-3 to hex [1225]"),
			new CommandLogItem(2, "Move TB-1 to hex [1328]"),
			new CommandLogItem(2, "Move TB-2 to hex [1328]"),
			new CommandLogItem(2, "Move LB-1 to hex [1115]"),
			new CommandLogItem(2, "Move BB-1 to hex [1213]"),
			new CommandLogItem(2, "Move BB-1 to hex [1213]"),
			new ChatLogItem(2, " X This is a really long message that takes more than two lines and needs to expand to accomodate all content"),
			};
	}
	*/
}


using System;
using System.Collections.Generic;

namespace Client
{
	public class m_festival_lucky_bag_rank_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 id = 0;	//optional int32	活动id

		public Int32 times = 0;	//optional int32	剩余免费次数

		public p_rank_lucky_bag[] rank_list = null;	//repeated p_rank_lucky_bag

		public Int32 my_rank = 0;	//optional int32

		public p_lucky_bag_history[] history_list = null;	//repeated p_lucky_bag_history 历史记录

		public override int __ID(){ return 1816; }
	}

	public class m_festival_pay_get_rank_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 id = 0;	//optional int32	活动id

		public p_rank_pay[] rank_list = null;	//repeated p_rank_pay

		public Int32 my_rank = 0;	//optional int32

		public override int __ID(){ return 1817; }
	}

	public class m_festival_snow_use_item_toc
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public long role_id = 0;	//optional int64		角色ID

		public p_goods[] update_goods = null;	//repeated p_goods	更新背包

	}

	public class m_festival_snow_hide_toc
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

	}

	public class m_festival_snow_notice_toc
	{
		public p_msg notice = null;	//required p_msg

	}

	public class m_festival_card_send_roles_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 id = 0;	//optional int32	活动id

		public long[] role_list = null;	//repeated int64	已赠送的玩家

		public override int __ID(){ return 1819; }
	}

	public class m_festival_card_use_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool			操作成功

		public p_msg reason = null;	//optional p_msg			操作失败原因

		public p_goods[] update_goods = null;	//repeated p_goods		更新背包

		public long role_id = 0;	//optional int64			赠送的对象

		public p_client_good[] show_list = null;	//repeated p_client_good	展示的物品

		public override int __ID(){ return 1820; }
	}

	public class m_festival_card_show_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool		操作成功

		public p_msg reason = null;	//optional p_msg		操作失败原因

		public p_card_show_letter[] card_list = null;	//repeated p_card_show_letter	贺卡展示

		public override int __ID(){ return 1821; }
	}

	public class m_festival_card_get_info_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 page_id = 0;	//optional int32	页数

		public p_card_letter[] card_list = null;	//repeated p_card_letter 自己的贺卡列表

		public Int32 total_num = 0;	//optional int32		贺卡总数

		public override int __ID(){ return 1822; }
	}

	public class m_festival_card_new_toc : BaseToSC
	{
		public Int32 id = 0;	//required	int32			活动id

		public Int32 type = 1;	//required 	int32  			1:在线  2：登陆

		public p_card_letter new_card = null;	//required 	p_card_letter 	新贺卡

		public override int __ID(){ return 1823; }
	}

	public class m_festival_card_read_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool	操作成功

		public p_msg reason = null;	//optional p_msg 操作失败原因

		public Int32 seq = 0;	//optional int32	贺卡id

		public override int __ID(){ return 1824; }
	}

	public class m_festival_card_delete_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool		操作成功

		public p_msg reason = null;	//optional p_msg		操作失败原因

		public Int32 seq = 0;	//optional int32 	删除贺卡seq

		public Int32 page_id = 0;	//optional int32		第几页

		public p_card_letter[] card_list = null;	//repeated p_card_letter 	接收贺卡列表

		public Int32 total_num = 0;	//optional int32		贺卡总数

		public override int __ID(){ return 1825; }
	}

	public class m_festival_egg_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 open_times = 0;	//optional int32 当前开启的次数，跟重置类型相关

		public Int32 open_fee = 0;	//optional int32 本次开启费用

		public Int32 reset_type = 0;	//optional int32 重置类型，1：不重置，2：每日重置

		public p_egg_award[] open_list = null;	//repeated p_egg_award 已经打开的奖励

		public Int32 left_time = 0;	//optional int32 剩余时间

		public Int32 big_award = 0;	//optional int32 大奖的皮肤id

		public override int __ID(){ return 1826; }
	}

	public class m_festival_egg_open_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] up_goods = null;	//repeated p_goods 更新背包

		public Int32 open_fee = 0;	//optional int32 下一次的费用

		public Int32 open_times = 0;	//optional int32 当前开启的次数，跟重置类型相关

		public p_egg_award award = null;	//optional p_egg_award 本次奖励

		public override int __ID(){ return 1827; }
	}

	public class m_rank_silver_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public Int32 rank_time = 0;	//optional int32	下次重排时间

		public Int32 my_rank = 0;	//optional int32	我的排行，0是未上榜

		public Int32 page_id = 0;	//optional int32	第几页

		public Int32 page_count = 0;	//optional int32	总页数

		public p_ranking_silver[] rank_list = null;	//repeated p_ranking_silver

		public override int __ID(){ return 903; }
	}

	public class m_pvp5_create_queue_toc : BaseToSC
	{
		public Boolean succ = false;	//required  bool

		public p_msg reason = null;	//optional  p_msg

		public Int32 type = 0;	//optional  int32

		public Int32 corps_level = 0;	//optional  int32

		public override int __ID(){ return 1902; }
	}

	public class m_pvp5_invite_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long role_id = 0;	//optional int64

		public string role_name = "";	//optional string

		public Int32 dan_grading = 0;	//optional	int32  	段位

		public Int32 high_grading = 0;	//optional	int32  	最高段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public string head_addr = "";	//optional	string	头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 type = 1;	//optional  int32

		public override int __ID(){ return 1903; }
	}

	public class m_pvp5_invite_result_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public Boolean is_agree = true;	//optional  bool

		public long role_id = 0;	//optional int64

		public string role_name = "";	//optional string

		public Int32 type = 1;	//optional  int32

		public p_pvp5_role_info[] roles = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info 自定义比赛的下面一方

		public Int32 corps_level = 0;	//optional  int32

		public override int __ID(){ return 1904; }
	}

	public class m_pvp5_kick_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long role_id = 0;	//optional int64

		public string role_name = "";	//optional string

		public p_pvp5_role_info[] roles = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info 自定义比赛的下面一方

		public override int __ID(){ return 1905; }
	}

	public class m_pvp5_quit_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long role_id = 0;	//optional int64

		public string role_name = "";	//optional string

		public p_pvp5_role_info[] roles = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info 自定义比赛的下面一方

		public override int __ID(){ return 1906; }
	}

	public class m_pvp5_match_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 time = 0;	//optional  int32 开始匹配的时间戳

		public Int32 type = 1;	//optional  int32  1、对抗赛 2、排位赛 3、练习赛入门  4、练习赛简单 5、练习赛一般

		public p_pvp5_role_info[] roles = null;	//repeated p_pvp5_role_info

		public override int __ID(){ return 1907; }
	}

	public class m_pvp5_cancel_match_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long role_id = 0;	//optional  int64 退出者

		public Int32 type = 1;	//optional  int32

		public p_pvp5_role_info[] roles = null;	//repeated  p_pvp5_role_info 剩下的人，第一个为队长

		public p_pvp5_role_info[] roles2 = null;	//repeated p_pvp5_role_info 自定义比赛的下面一方

		public override int __ID(){ return 1908; }
	}

	//匹配到对手和队友
	public class m_pvp5_match_result_toc : BaseToSC
	{
		public Int32 time = 0;	//optional  int32  确认截止时间

		public Int32 type = 1;	//optional  int32

		public string chat_name = "";	//optional string

		public p_pvp5_role_info[] roles1 = null;	//repeated 	p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated 	p_pvp5_role_info

		public p_player[] role_player = null;	//repeated p_player 已选的玩家

		public override int __ID(){ return 1909; }
	}

	//
	public class m_pvp5_fight_state_toc : BaseToSC
	{
		public Int32 state = 0;	//optional  int32  1 确定中， 2选英雄

		public Int32 time = 0;	//optional  int32  确认截止时间

		public Int32 type = 1;	//optional  int32

		public p_pvp5_role_info[] roles1 = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info

		public long[] confirm_roles = null;	//repeated  int64 确认的玩家

		public p_player[] roles = null;	//repeated  p_player 已选的玩家

		public override int __ID(){ return 1921; }
	}

	public class m_pvp5_fight_confirm_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public Boolean begin_select = false;	//optional bool 是否开始选英雄等

		public Int32 time = 0;	//optional  int32 时间戳

		public long[] roles = null;	//repeated  int64 确认的玩家

		public override int __ID(){ return 1910; }
	}

	public class m_pvp5_no_confirm_toc : BaseToSC
	{
		public Boolean succ = false;	//required  bool

		public p_msg reason = null;	//optional  p_msg

		public long[] role_ids = null;	//repeated  int64 没有确认的玩家

		public Boolean is_match = false;	//optional  bool 是否返回匹配

		public Int32 time = 0;	//optional  int32

		public Int32 type = 1;	//optional  int32

		public p_pvp5_role_info[] roles = null;	//repeated  p_pvp5_role_info 自己所在队列

		public override int __ID(){ return 1911; }
	}

	public class m_pvp5_hero_select_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public p_player[] role = null;	//repeated p_player 已选的玩家

		public override int __ID(){ return 1912; }
	}

	//
	//-record(m_pvp5_get_history_tos,{}).
	//-record(m_pvp5_get_history_toc,
	//{
	//	fight_info		%repeated  p_fight_info
	//}).
	public class m_pvp5_fight_result_toc : BaseToSC
	{
		public Int32 add_exp = 0;	//optional   int32 	增加经验

		public Int32 win_exp = 0;	//optional   int32 	首胜经验

		public Int32 double_exp = 0;	//optional   int32 	双倍经验

		public Int32 add_gold = 0;	//optional   int32 	增加金币

		public Int32 win_gold = 0;	//optional   int32 	首胜金币

		public Int32 double_gold = 0;	//optional   int32 	双倍金币

		public Int32 week_gold = 0;	//optional   int32 	本周获得金币

		public Int32 dan_grading = 0;	//optional   int32   段位

		public Int32 star = 0;	//optional   int32   星数

		public Int32 new_dan_grading = 0;	//optional   int32   段位

		public Int32 new_star = 0;	//optional   int32   星数

		public p_kvi[] items = null;	//repeated	p_kvi   获得的道具 key是道具ID value是数量

		public Int32 box_id = 0;	//optional   int32   宝箱ID 0表示没有获得 -1表示位置满了

		public Int32 win_first_time = 0;	//optional   int32   下次首胜时间戳 0或者小于当前是可以获得首胜了

		public Boolean first_box = false;	//optional   bool   是否第一个宝箱

		public Boolean share_award = false;	//optional   bool   是否开启分享

		public Int32 add_score = 0;	//optional	int32	当局增加的超能积分

		public Int32 new_score = 0;	//optional	int32	当前的超能积分

		public Int32 old_score = 0;	//optional	int32	上次的超能积分

		public Int32 add_star = 0;	//optional	int32	1表示当前积分多加了一个星数

		public Int32 l_win_sc = 0;	//optional	int32	连胜/虽败犹荣积分

		public Boolean win = true;	//optional	bool

		public Int32 fight_id = 0;	//optional	int32	战斗id

		public Int32 add_use_exp = 0;	//optional	int32	增加的熟练度

		public Int32 hero_type = 0;	//optional  int32

		public override int __ID(){ return 1916; }
	}

	public class m_pvp5_get_result_toc : BaseToSC
	{
		public Int32 win_times = 0;	//optional   int32 	胜利场次

		public Int32 total_times = 0;	//optional   int32 	总场次

		public Int32 mvp_times = 0;	//optional   int32 	全场最佳

		public Int32 dashen_num = 0;	//optional   int32   大神次数

		public Int32 chaoshen_num = 0;	//optional   int32   超神次数

		public Int32 kill3_num = 0;	//optional   int32 	三杀次数

		public Int32 kill4_num = 0;	//optional   int32 	四杀次数

		public Int32 kill5_num = 0;	//optional   int32 	五杀次数

		public p_pvp5_fight_result[] result_list = null;	//repeated  p_pvp5_fight_result

		public override int __ID(){ return 1915; }
	}

	public class m_pvp5_match_rank_info_toc : BaseToSC
	{
		public Int32 season_num = 0;	//optional   int32    第几赛季

		public Int32 dan_grading = 0;	//optional   int32   段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 star = 0;	//optional   int32   星数

		public Int32 begin_time = 0;	//optional   int32 	开始时间

		public Int32 end_time = 0;	//optional   int32 	结束时间

		public Int32 win_times = 0;	//optional   int32 	胜利场次

		public Int32 times = 0;	//optional   int32 	总场次

		public Int32 max_win = 0;	//optional   int32 	最高连胜场次

		public Int32 hero_type = 0;	//optional   int32 	最近一次的英雄

		public Boolean is_win = false;	//optional   bool 	最近一次输赢

		public Int32 time = 0;	//optional   int32  	最近一次时间

		public Int32 score = 0;	//optional	int32   超能积分

		public override int __ID(){ return 1922; }
	}

	public class m_pvp5_diy_chg_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_pvp5_role_info[] roles1 = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info 自定义比赛的下面一方

		public override int __ID(){ return 1917; }
	}

	public class m_pvp5_diy_add_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_pvp5_role_info[] roles1 = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info 自定义比赛的下面一方

		public override int __ID(){ return 1918; }
	}

	public class m_pvp5_diy_start_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 time = 0;	//optional  int32 确认截止时间

		public string chat_name = "";	//optional string

		public p_pvp5_role_info[] roles1 = null;	//repeated  p_pvp5_role_info

		public p_pvp5_role_info[] roles2 = null;	//repeated  p_pvp5_role_info 自定义比赛的下面一方

		public override int __ID(){ return 1919; }
	}

	public class m_pvp5_diy_disband_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public override int __ID(){ return 1920; }
	}

	//加载结束通知
	public class m_pvp5_load_finish_toc : BaseToSC
	{
		public long role_id = 0;	//optional int64 角色ID

		public override int __ID(){ return 1913; }
	}

	public class m_pvp5_report_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public long dest_role = 0;	//optional int64

		public override int __ID(){ return 1924; }
	}

	public class m_pvp5_day_report_toc : BaseToSC
	{
		public Int32 times = 0;	//optional	int32 战斗场次

		public Int32 kill_num = 0;	//optional	int32 击杀数

		public Int32 kill_rank = 0;	//optional	int32 击杀排名  千分比，即显示的时候有一位小数点

		public Int32 ass_times = 0;	//optional	int32

		public Int32 ass_rank = 0;	//optional	int32	助攻排名  千分比，即显示的时候有一位小数点

		public Int32 dead_times = 0;	//optional   int32

		public Int32 dead_rank = 0;	//optional   int32	死亡排名  千分比，即显示的时候有一位小数点

		public p_kvi[] random_list = null;	//repeated 	p_kvi   key 1是最长时间，单位秒  2是最短时间 3是杀人最多数 4是死亡最多数 5是连胜最多数

		public p_ranking_role_info[] rank_list = null;	//repeated  	p_ranking_role_info   ranking 分别为  30001，天全场最佳排行榜 30002，天击杀排行榜 30003，天死亡排行榜 30004，天助攻排行榜 30005，天在线排行榜  30006，天花费源石排行榜

		public Int32 zan_num = 0;	//optional   int32	点赞数

		public override int __ID(){ return 1925; }
	}

	public class m_pvp5_dian_zan_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public override int __ID(){ return 1926; }
	}

	public class m_fcm_set_toc : BaseToSC
	{
		public Boolean succ = false;	//required	bool

		public p_msg reason = null;	//optional	p_msg

		public override int __ID(){ return 2002; }
	}

	public class m_fcm_info_toc : BaseToSC
	{
		public Int32 total_time = 0;	//optional	int32

		public override int __ID(){ return 2003; }
	}

	public class m_fcm_notice_toc : BaseToSC
	{
		public Boolean passed = false;	//optional	bool  是否已经验证过

		public Int32 total_online_time = 0;	//optional	int32

		public override int __ID(){ return 2004; }
	}

	//登陆主动推送
	public class m_story_info_toc : BaseToSC
	{
		public p_kvi[] list = null;	//repeated p_kvi

		public Int32[] sprog_list = null;	//repeated int32

		public override int __ID(){ return 2102; }
	}

	public class m_story_update_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public Int32 key = 0;	//optional int32

		public Int32 value = 0;	//optional int32

		public override int __ID(){ return 2103; }
	}

	public class m_story_sprog_update_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public Int32 id = 0;	//optional int32

		public override int __ID(){ return 2104; }
	}

	public class m_treasure_box_get_toc : BaseToSC
	{
		public p_role_treasure_box[] box_list = null;	//repeated 	p_role_treasure_box

		public override int __ID(){ return 2202; }
	}

	public class m_treasure_box_deblocking_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public p_role_treasure_box box_info = null;	//optional	p_role_treasure_box

		public override int __ID(){ return 2203; }
	}

	public class m_treasure_box_open_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 type_id = 0;	//optional   int32

		public Int32 silver = 0;	//optional	int32

		public Int32 gold = 0;	//optional	int32

		public Int32 bind_gold = 0;	//optional	int32

		public Int32 wafer_chip = 0;	//optional	int32

		public p_kvi[] goods = null;	//repeated   p_kvi	获得的物品

		public p_goods[] goods_list = null;	//repeated   p_goods	获得的物品

		public p_chip_info[] chip_list = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 2204; }
	}

	public class m_sweet_share_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 add_sweet = 0;	//optional   int32

		public override int __ID(){ return 2302; }
	}

	public class m_sweet_buy_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public p_client_good[] award_list = null;	//repeated   p_client_good	获得列表

		public override int __ID(){ return 2303; }
	}

	public class m_sweet_qry_toc : BaseToSC
	{
		public Int32 week_count = 0;	//optional	int32 本周获得的积分

		public override int __ID(){ return 2304; }
	}

	public class m_sweet_fight_award_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 add_sweet = 0;	//optional   int32

		public override int __ID(){ return 2305; }
	}

	public class m_login_award_pay_flag_toc : BaseToSC
	{
		public Int32 first_pay = 0;	//optional	int32 0没充值，1首充可领，2首充已领

		public p_client_good[] award_list = null;	//repeated   p_client_good	获得列表

		public Int32[] list = null;	//repeated int32 里面是对应的源石

		public override int __ID(){ return 1705; }
	}

	public class m_login_award_pay_first_award_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public p_chip_info[] chip_list = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 1706; }
	}

	public class m_login_award_alloy_award_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 type = 0;	//optional	int32

		public Int32 add_alloy = 0;	//optional	int32 领导的合金

		public override int __ID(){ return 1707; }
	}

	public class m_login_award_alloy_notice_toc : BaseToSC
	{
		public Boolean day_alloy = false;	//required	bool	每天登陆可领取

		public Int32 montch_card_time = 0;	//optional	int32   月卡失效时间

		public Boolean montch_alloy = false;	//required	bool	月卡可领取

		public Int32 week_card_time = 0;	//optional	int32   周卡失效时间

		public Boolean week_alloy = false;	//required	bool 	周卡可领取

		public override int __ID(){ return 1708; }
	}

	public class m_egg_get_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 open_times = 0;	//optional int32 当前开启的次数，跟重置类型相关

		public Int32 open_fee = 0;	//optional int32 本次开启费用

		public Int32 reset_type = 0;	//optional int32 重置类型，1：不重置，2：每日重置

		public p_egg_award[] open_list = null;	//repeated p_egg_award 已经打开的奖励

		public Int32 left_time = 0;	//optional int32 剩余时间

		public Int32 big_award = 0;	//optional int32 大奖的皮肤id

		public override int __ID(){ return 2602; }
	}

	public class m_egg_open_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] up_goods = null;	//repeated p_goods 更新背包

		public Int32 open_fee = 0;	//optional int32 下一次的费用

		public Int32 open_times = 0;	//optional int32 当前开启的次数，跟重置类型相关

		public p_egg_award award = null;	//optional p_egg_award 本次奖励

		public override int __ID(){ return 2603; }
	}

	public class m_treasure_get_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32

		public Int32 lucky = 0;	//required int32 幸运值

		public Int32 times = 0;	//required int32 已经进行的次数

		public Int32[] get_award = null;	//repeated int32 已经领过的奖励id

		public p_treasure_award[] list = null;	//repeated p_treasure_award

		public override int __ID(){ return 2702; }
	}

	public class m_treasure_buy_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 0;	//optional int32

		public Int32 num_type = 0;	//optional int32

		public p_goods[] up_goods = null;	//repeated p_goods

		public p_treasure_award[] award_list = null;	//repeated p_treasure_award 获得的奖励

		public Int32 lucky = 0;	//optional int32

		public Int32 times = 0;	//optional int32

		public Int32[] get_award = null;	//repeated int32 已经领过的奖励id

		public p_chip_info[] chip_list = null;	//repeated p_chip_info 更新芯片背包

		public override int __ID(){ return 2703; }
	}

	public class m_treasure_get_award_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 0;	//optional int32

		public Int32 id = 0;	//optional int32

		public p_goods[] up_goods = null;	//repeated p_goods

		public p_treasure_award[] award_list = null;	//repeated p_treasure_award 获得的奖励

		public Int32 times = 0;	//optional int32

		public Int32[] get_award = null;	//repeated int32 已经领过的奖励id

		public p_chip_info[] chip_list = null;	//repeated p_chip_info 更新芯片背包

		public override int __ID(){ return 2704; }
	}

	public class m_chip_qry_toc : BaseToSC
	{
		public Int32 use_id = 0;	//required   int32  默认使用的页码

		public p_chip_page[] page_list = null;	//repeated	p_chip_page

		public p_chip_info[] chip_bag = null;	//repeated	p_chip_info

		public Int32 lottery_free_time = 0;	//optional    int32    免费合金抽时间戳

		public Int32 times = 0;	//optional   int32 金币抽距离保底次数

		public Int32 gold_times = 0;	//optional   int32 合金抽距离保底次数

		public Int32[] open_list = null;	//repeated	int32

		public override int __ID(){ return 2402; }
	}

	public class m_chip_chg_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 page_id = 0;	//optional    int32   芯片页码

		public override int __ID(){ return 2408; }
	}

	public class m_chip_load_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public p_chip_info[] up_chip_bag = null;	//repeated	p_chip_info

		public p_chip_page page_info = null;	//optional	p_chip_page

		public override int __ID(){ return 2403; }
	}

	public class m_chip_unload_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public p_chip_info[] up_chip_bag = null;	//repeated	p_chip_info

		public p_chip_page page_info = null;	//optional	p_chip_page

		public override int __ID(){ return 2404; }
	}

	public class m_chip_open_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public p_chip_page page_info = null;	//optional	p_chip_page

		public override int __ID(){ return 2405; }
	}

	public class m_chip_buy_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 type_id = 0;	//optional    int32

		public Int32 num = 0;	//optional    int32

		public p_chip_info[] chip_info = null;	//repeated    p_chip_info

		public override int __ID(){ return 2406; }
	}

	public class m_chip_sell_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 add_chip = 0;	//optional    int32  获得的晶片

		public p_chip_info[] chip_bag = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 2407; }
	}

	public class m_chip_chg_notice_toc : BaseToSC
	{
		public p_kvi[] use_list = null;	//repeated    p_kvi	 key是hero_type value是页码

		public override int __ID(){ return 2409; }
	}

	public class m_chip_chg_name_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 page_id = 0;	//optional    int32   芯片页码

		public string page_name = "";	//optional    string

		public override int __ID(){ return 2410; }
	}

	public class m_chip_lottery_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 type = 0;	//optional   int32	 次数

		public Int32 next_time = 0;	//optional   int32	 下次免费时间

		public Int32[] list = null;	//repeated   int32	抽到的芯片

		public p_chip_info[] chip_list = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 2411; }
	}

	public class m_chip_open_slot_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 pos_id = 0;	//optional   int32

		public override int __ID(){ return 2412; }
	}

	public class m_chip_up_bag_toc : BaseToSC
	{
		public p_chip_info[] up_bag = null;	//repeated p_chip_info 更新背包

		public override int __ID(){ return 2413; }
	}

	public class m_vip_get_award_info_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32[] taken_list = null;	//repeated int32 已领免费奖励的VIP等级

		public Int32[] buy_list = null;	//repeated int32 已购买的vip等级

		public override int __ID(){ return 2502; }
	}

	public class m_vip_take_award_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 0;	//optional int32   1是免费，2是买

		public Int32 vip_grade = 0;	//optional int32 vip等级

		public p_goods[] goods_list = null;	//repeated p_goods 奖励的物品

		public p_chip_info[] chip_bag = null;	//repeated p_chip_info 芯片背包变化的芯片信息

		public override int __ID(){ return 2503; }
	}

}

using System;
using System.Collections.Generic;

namespace Client
{
	public class m_client_login_tos : BaseToSC
	{
		public string account = "";	//optional	string	账号

		public Int32 server_id = 0;	//required	int32	服务器id

		public Int32 agent_id = 0;	//required	int32	代理商id

		public Int32 sdk_type = 0;	//optional	int32	sdk类型，1蜂鸟sdk（特别处理）

		public Boolean is_release = false;	//required   bool	是否正式版本

		public string name = "";	//optional	string	接蜂鸟sdk时用  韩服是username

		public string token = "";	//optional	string	平台token, 接蜂鸟sdk时即uid 韩服是验证串

		public string ext = "";	//optional	string	接蜂鸟sdk时用

		public string client_version = "";	//required	string

		public string device_id = "";	//required	string	设备ID

		public string client_info = "";	//required	string	客户端信息（用于日志）

		public Int32 time = 0;	//optional	int32	登录时间戳  韩服用

		public override int __ID(){ return 202; }
	}

	//选择角色
	public class m_client_select_tos : BaseToSC
	{
		public Int32 device_type = 0;	//required	int32	1安卓 2ios 3 web 4 pc

		public long role_id = 0;	//required	int64	%选中的角色ID,传0表示取上一次登陆的角色

		public Int32 lang_id = 0;	//required	int32	语言id

		public override int __ID(){ return 215; }
	}

	//建立角色并进入游戏
	public class m_client_create_role_name_tos : BaseToSC
	{
		public string[] role_name_list = null;	//repeated	string

		public override int __ID(){ return 223; }
	}

	public class m_client_create_role_tos : BaseToSC
	{
		public string rolename = "";	//required	string

		public string head_addr = "";	//optional	string	头像路径

		public Int32 sex = 0;	//required	int32

		public Int32 hero_id = 0;	//required   int32

		public string ext = "";	//optional	string

		public Int32 device_type = 0;	//required	int32	1安卓 2ios 3 web 4 pc

		public Int32 lang_id = 0;	//required	int32

		public override int __ID(){ return 203; }
	}

	public class m_client_exe_cmd_tos : BaseToSC
	{
		public Int32 type = 0;	//required	int32 默认类型为0，表示客户端测试命令，1为服务端测试命令

		public string cmd = "";	//required	string

		public override int __ID(){ return 205; }
	}

	public class m_client_logout_tos : BaseToSC
	{
		public override int __ID(){ return 207; }
	}

	public class m_client_get_role_icon_tos : BaseToSC
	{
		public Int32[] id_list = null;	//repeated	int32

		public override int __ID(){ return 206; }
	}

	public class m_client_bind_account_tos : BaseToSC
	{
		public string[] account_name = null;	//repeated	string  待绑定的平台帐号名

		public string[] account_pwd = null;	//repeated	string  待绑定的平台帐号密码

		public override int __ID(){ return 209; }
	}

	public class m_client_gm_complaint_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32 类型 1游戏问题 2游戏建议 3游戏投诉 4游戏广告

		public string content = "";	//required string 内容

		public string picture_url = "";	//optional string 拼凑的图片url 以;隔开

		public override int __ID(){ return 210; }
	}

	public class m_client_reconnect_tos : BaseToSC
	{
		public override int __ID(){ return 211; }
	}

	//登入公告
	public class m_client_login_notice_tos : BaseToSC
	{
		public override int __ID(){ return 214; }
	}

	//登入公告
	public class m_client_unlink_account_tos : BaseToSC
	{
		public override int __ID(){ return 220; }
	}

	//切到后台通知服务端
	public class m_client_hide_tos : BaseToSC
	{
		public override int __ID(){ return 218; }
	}

	//限号激活
	public class m_client_tst_card_tos : BaseToSC
	{
		public string card_name = "";	//required	string  测试卡生成字符串

		public string account = "";	//required	string  账号名(login_toc中返回的)

		public override int __ID(){ return 219; }
	}

	public class m_bag_get_bag_info_tos : BaseToSC
	{
		public override int __ID(){ return 302; }
	}

	public class m_bag_sell_goods_tos : BaseToSC
	{
		public Int32 goods_id = 0;	//required   int32   出售的物品ID

		public Int32 num = 0;	//required   int32   物品数量

		public override int __ID(){ return 303; }
	}

	public class m_bag_buy_back_tos : BaseToSC
	{
		public Int32 goods_id = 0;	//required   int32   回购的物品ID

		public override int __ID(){ return 305; }
	}

	public class m_bag_compose_goods_tos : BaseToSC
	{
		public Int32 goods_id = 0;	//required int32  物品id

		public Int32 type = 0;	//required int32  合成个数

		public override int __ID(){ return 306; }
	}

	public class m_bag_dec_goods_tos : BaseToSC
	{
		public Int32[] goods_id = null;	//repeated int32

		public override int __ID(){ return 307; }
	}

	//多个物品合成一个物品
	public class m_bag_multiple_compose_goods_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32

		public override int __ID(){ return 308; }
	}

	public class m_bag_exchange_money_tos : BaseToSC
	{
		public Int32 money = 0;	//required   int32  兑换的数量

		public Int32 type = 0;	//optional 	 int32 兑换的类型，金币转银币 type 是1，钻石转银币是2

		public override int __ID(){ return 309; }
	}

	public class m_item_use_tos : BaseToSC
	{
		public Int32 goods_id = 0;	//required   int32   使用的物品ID

		public Int32 use_num = 0;	//required   int32   物品数量

		public Int32 other = 0;	//optional   int32

		public override int __ID(){ return 402; }
	}

	//查询商城的子商店
	public class m_shop_shops_list_tos : BaseToSC
	{
		public override int __ID(){ return 602; }
	}

	//查询商店
	public class m_shop_get_goods_tos : BaseToSC
	{
		public Int32 npc_id = 0;	//optional int32  npc_id 0表示商城

		public Int32 shop_id = 0;	//required int32  商店id

		public override int __ID(){ return 603; }
	}

	public class m_shop_buy_tos : BaseToSC
	{
		public Int32 shop_id = 0;	//required int32  商店id

		public Int32 goods_id = 0;	//required int32  商品ID

		public Int32 goods_num = 1;	//required int32  商品的数量

		public Int32 buy_type = 0;	//required int32	price_type=2的时候传对应的货币类型过来

		public Int32 del_goods_id = 0;	//required int32	使用的打折卷

		public override int __ID(){ return 604; }
	}

	public class m_shop_dial_info_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32		%%哪种类型的转盘抽奖

		public override int __ID(){ return 605; }
	}

	public class m_shop_buy_dial_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32		%%哪种类型的转盘抽奖

		public override int __ID(){ return 606; }
	}

	public class m_shop_get_goods_by_type_id_tos : BaseToSC
	{
		public Int32 shop_id = 0;	//required int32  商店id

		public Int32 type_id = 0;	//required int32	道具id

		public override int __ID(){ return 607; }
	}

	public class m_letter_send_letter_tos : BaseToSC
	{
		public long recv_id = 0;	//required int64  	收信人id

		public string title = "";	//optional string  	标题

		public string text = "";	//required string  	正文

		public override int __ID(){ return 502; }
	}

	public class m_letter_new_count_tos : BaseToSC
	{
		public override int __ID(){ return 508; }
	}

	public class m_letter_open_mailbox_tos : BaseToSC
	{
		public Int32 page_size = 6;	//required int32	每页条数

		public Int32 page_id = 1;	//required int32	第几页

		public override int __ID(){ return 504; }
	}

	public class m_letter_delete_letter_tos : BaseToSC
	{
		public Int32 seq = 0;	//required int32	邮件id 0表示删除所有已读的空邮件

		public Int32 page_size = 6;	//required int32	每页条数

		public Int32 page_id = 1;	//required int32	第几页

		public override int __ID(){ return 506; }
	}

	public class m_letter_read_letter_tos : BaseToSC
	{
		public Int32 seq = 0;	//required int32	邮件id

		public override int __ID(){ return 505; }
	}

	public class m_letter_get_attach_tos : BaseToSC
	{
		public Int32 seq = 0;	//required int32	邮件id

		public override int __ID(){ return 507; }
	}

	public class m_letter_get_all_attach_tos : BaseToSC
	{
		public override int __ID(){ return 509; }
	}

	//--------------------------------任务----------------------------
	public class m_task_update_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32   id

		public Int32 type = 1;	//required int32

		public Int32 num = 0;	//required int32	  更新的进度

		public override int __ID(){ return 702; }
	}

	//暂时没用
	public class m_task_accept_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32   id  0:黑耀任务   -1：护送任务

		public override int __ID(){ return 703; }
	}

	public class m_task_finish_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32   id

		public override int __ID(){ return 704; }
	}

	public class m_task_freshen_tos : BaseToSC
	{
		public Int32 task_id = 0;	//required int32

		public override int __ID(){ return 706; }
	}

	public class m_task_star_award_tos : BaseToSC
	{
		public Int32 star = 0;	//required int32

		public override int __ID(){ return 707; }
	}

	public class m_hero_get_heros_tos : BaseToSC
	{
		public override int __ID(){ return 804; }
	}

	public class m_hero_head_upload_tos : BaseToSC
	{
		public string addr = "";	//required string	头像地址

		public override int __ID(){ return 805; }
	}

	public class m_hero_headstr_tmp_tos : BaseToSC
	{
		public string addr = "";	//required string	头像地址

		public override int __ID(){ return 815; }
	}

	public class m_hero_use_headframe_tos : BaseToSC
	{
		public Int32 head_frame_id = 0;	//required int32

		public override int __ID(){ return 817; }
	}

	public class m_hero_sign_str_tos : BaseToSC
	{
		public string str = "";	//required string	签名

		public override int __ID(){ return 806; }
	}

	public class m_hero_chg_skin_tos : BaseToSC
	{
		public Int32 hero_type = 0;	//required int32  英雄id

		public Int32 skin_id = 0;	//required int32  皮肤ID

		public override int __ID(){ return 809; }
	}

	public class m_hero_equip_chg_tos : BaseToSC
	{
		public Int32 hero_type = 0;	//required int32  英雄id

		public p_equip_info equip_info = null;	//required p_equip_info

		public Int32 type = 0;	//optional int32  前端用

		public override int __ID(){ return 810; }
	}

	public class m_hero_get_roleinfo_tos : BaseToSC
	{
		public long role_id = 0;	//required int64

		public override int __ID(){ return 811; }
	}

	public class m_hero_set_qry_tos : BaseToSC
	{
		public Int32 type = 0;	//optional int32  0 允许，1不允许

		public override int __ID(){ return 812; }
	}

	//改名
	public class m_hero_change_name_tos : BaseToSC
	{
		public string role_name = "";	//required string

		public override int __ID(){ return 814; }
	}

	//获取排行榜
	public class m_rank_get_tos : BaseToSC
	{
		public Int32 rank_id = 0;	//required int32	排行榜ID

		public long family_id = 0;	//required int64	帮会ID,没有为0

		public Int32 page_size = 10;	//required int32	每页条数

		public Int32 page_id = 1;	//required int32	第几页

		public Int32 my_rank = 0;	//optional int32

		public override int __ID(){ return 902; }
	}

	public class m_friend_qry_role_tos : BaseToSC
	{
		public string role_name = "";	//required   string  角色名称

		public override int __ID(){ return 1006; }
	}

	public class m_friend_querry_info_tos : BaseToSC
	{
		public override int __ID(){ return 1002; }
	}

	public class m_friend_querry_rank_tos : BaseToSC
	{
		public override int __ID(){ return 1011; }
	}

	public class m_friend_add_tos : BaseToSC
	{
		public long role_id = 0;	//required int64 玩家id

		public string msg = "";	//required string 验证信息

		public override int __ID(){ return 1003; }
	}

	public class m_friend_add_result_tos : BaseToSC
	{
		public long role_id = 0;	//required int64 玩家id

		public Boolean is_agree = false;	//required  bool

		public override int __ID(){ return 1004; }
	}

	public class m_friend_delete_tos : BaseToSC
	{
		public long role_id = 0;	//optional int64 玩家id

		public override int __ID(){ return 1005; }
	}

	public class m_friend_near_get_tos : BaseToSC
	{
		public Int32 level = 0;	//optional int32

		public override int __ID(){ return 1008; }
	}

	public class m_friend_near_cancel_tos : BaseToSC
	{
		public override int __ID(){ return 1010; }
	}

	//创建战队
	public class m_corps_create_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32  1金币创建 2，源石创建

		public string corps_name = "";	//required string

		public string notice = "";	//required string

		public p_kvi[] join_condition = null;	//repeated p_kvi   加入条件 1有等级要求，2段位要求

		public override int __ID(){ return 1302; }
	}

	// 战队列表
	public class m_corps_list_tos : BaseToSC
	{
		public Int32 page_size = 20;	//required int32	每页条数

		public Int32 page_id = 1;	//required int32	第几页

		public override int __ID(){ return 1303; }
	}

	// 战队排行榜
	public class m_corps_rank_tos : BaseToSC
	{
		public override int __ID(){ return 1328; }
	}

	// 战队查询
	public class m_corps_qry_tos : BaseToSC
	{
		public string key = "";	//optional string

		public override int __ID(){ return 1304; }
	}

	//查询战队信息
	public class m_corps_getinfo_tos : BaseToSC
	{
		public long corps_id = 0;	//required int64

		public override int __ID(){ return 1314; }
	}

	//签到
	public class m_corps_sign_tos : BaseToSC
	{
		public override int __ID(){ return 1305; }
	}

	//申请加入本战队的列表
	public class m_corps_request_list_tos : BaseToSC
	{
		public long corps_id = 0;	//required int64

		public override int __ID(){ return 1306; }
	}

	//申请加入
	public class m_corps_request_tos : BaseToSC
	{
		public long corps_id = 0;	//required int64

		public override int __ID(){ return 1307; }
	}

	//一键申请入队
	public class m_corps_request_all_tos : BaseToSC
	{
		public override int __ID(){ return 1308; }
	}

	//同意/拒绝加入
	public class m_corps_accept_tos : BaseToSC
	{
		public long role_id = 0;	//required int64

		public Int32 type = 1;	//required int32 1同意，2拒绝，3清空

		public override int __ID(){ return 1309; }
	}

	//修改公告,入会条件
	public class m_corps_change_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32 1修改入会条件， 2修改公告

		public p_kvi[] join_condition = null;	//repeated p_kvi

		public string notice = "";	//optional string

		public override int __ID(){ return 1310; }
	}

	//修改自动审批chg_agree
	public class m_corps_chg_agree_tos : BaseToSC
	{
		public Boolean auto_agree = false;	//optional bool 自动审批

		public override int __ID(){ return 1319; }
	}

	//退出战队
	public class m_corps_quit_tos : BaseToSC
	{
		public override int __ID(){ return 1311; }
	}

	//踢人
	public class m_corps_kick_tos : BaseToSC
	{
		public long[] role_ids = null;	//repeated int64

		public override int __ID(){ return 1312; }
	}

	//改官职
	public class m_corps_change_job_tos : BaseToSC
	{
		public long role_id = 0;	//required int64

		public Int32 type = 0;	//required int32	1指定为副队长，2取消副队长，3转移会长

		public override int __ID(){ return 1313; }
	}

	//战队商店
	public class m_corps_shop_tos : BaseToSC
	{
		public override int __ID(){ return 1316; }
	}

	//战队商店购买
	public class m_corps_shop_buy_tos : BaseToSC
	{
		public Int32 type_id = 0;	//required int32  商品ID

		public Int32 num = 1;	//required int32  商品的数量

		public override int __ID(){ return 1317; }
	}

	//战队聊天记录
	public class m_corps_get_chatinfo_tos : BaseToSC
	{
		public override int __ID(){ return 1318; }
	}

	//查询系统红包
	public class m_corps_get_sys_hongbao_tos : BaseToSC
	{
		public override int __ID(){ return 1325; }
	}

	//战队发红包
	public class m_corps_hongbao_send_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public Int32 gold = 0;	//required int32

		public Int32 num = 0;	//required int32

		public string hongbao_desc = "";	//required  string

		public override int __ID(){ return 1323; }
	}

	//战队领红包
	public class m_corps_hongbao_get_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public override int __ID(){ return 1324; }
	}

	//查询战队红包
	public class m_corps_hongbao_qry_tos : BaseToSC
	{
		public override int __ID(){ return 1327; }
	}

	//组队界面查询好友状态
	public class m_corps_qry_state_tos : BaseToSC
	{
		public override int __ID(){ return 1329; }
	}

	//不要在voice_data后面加字段
	public class m_chat_user_msg_tos : BaseToSC
	{
		public Int32 msg_type = 0;	//optional int32

		public long target_id = 0;	//required int64	 接口者ID

		public string target_name = "";	//optional string 接收者名字

		public string text = "";	//optional string 文字消息

		public Boolean is_log = true;	//optional bool	是否记录

		public Int32 voice_time = 0;	//optional int32  语音时长(秒),为0表示非语音

		public byte[] voice_data = null;	//repeated byte   语音流

		public override int __ID(){ return 1102; }
	}

	public class m_chat_voice_query_tos : BaseToSC
	{
		public long voice_id = 0;	//required int64

		public override int __ID(){ return 1108; }
	}

	public class m_chat_team_msg_tos : BaseToSC
	{
		public long[] roles = null;	//repeated int64

		public Int32 type = 0;	//optional  int32 前端用于区分场景

		public Boolean is_log = true;	//optional bool	是否记录日志

		public string text = "";	//optional string 文字消息

		public override int __ID(){ return 1107; }
	}

	public class m_chat_loud_speaker_tos : BaseToSC
	{
		public Int32 color_id = 0;	//required int32	1:白色  2...

		public string text = "";	//required string

		public p_goods show_goods = null;	//optional p_goods

		public override int __ID(){ return 1109; }
	}

	//订阅上下线
	public class m_chat_subscription_state_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32		1订阅,2取消

		public long[] list = null;	//repeated int64		角色ID列表

		public override int __ID(){ return 1110; }
	}

	public class m_chat_reciver_msg_tos : BaseToSC
	{
		public long role_id = 0;	//required int64		角色ID

		public Int32 msg_id = 0;	//required int32		消息ID，-1表示删除聊天记录

		public override int __ID(){ return 1105; }
	}

	public class m_chat_broadcast_tos : BaseToSC
	{
		public p_msg msg = null;	//required p_msg

		public override int __ID(){ return 1112; }
	}

	// -record( p_role_achieve_finish,
	// {
	// 	id,			%required	int32
	// 	finish_time %optional	int32 成就完成时间, unix时间戳
	// }).
	//-record(m_achieve_trigger_tos,
	//{
	//	do_list,					%repeated	p_role_achieve	请求触发的成就列表
	//	need_doing_update=false   	%required bool 进行中的成就的话是否要给前端更新
	//}).
	//-record(m_achieve_trigger_toc,
	//{
	//	succ=true,	%required	bool	操作成功
	//	reason,		%optional p_msg	操作失败原因
	//	ach_list	%repeated	int32	 完成列表
	//}).
	public class m_achieve_update_tos : BaseToSC
	{
		public Int32 ach_type = 0;	//required	int32 成就类型

		public Int32 count = 0;	//required	int32 增加进度

		public override int __ID(){ return 1203; }
	}

	public class m_achieve_query_all_tos : BaseToSC
	{
		public override int __ID(){ return 1202; }
	}

	public class m_achieve_take_award_tos : BaseToSC
	{
		public Int32 num = 0;	//required	int32 领取的成就个数

		public override int __ID(){ return 1204; }
	}

	//客户端加载完毕以后，告知服务器
	public class m_fight_match_finish_reload_tos : BaseToSC
	{
		public Int32 progress = 0;	//optional	int32  进度

		public string version = "";	//optional string	版本

		public override int __ID(){ return 1406; }
	}

	public class m_fight_match_exit_tos : BaseToSC
	{
		public override int __ID(){ return 1403; }
	}

	public class m_fight_match_result_tos : BaseToSC
	{
		public Int32 winid = 0;	//required   int32     获胜的阵营

		public p_fight_result[] fight_result = null;	//repeated	p_fight_result	本场战斗结果

		public override int __ID(){ return 1404; }
	}

	public class m_fight_match_end_check_tos : BaseToSC
	{
		public Int32 kill_tower = 0;	//required   int32     推塔数

		public Int32 red_hp = 0;	//required   int32     红方血量

		public Int32 bule_hp = 0;	//required	int32	蓝方血量

		public override int __ID(){ return 1419; }
	}

	public class m_fight_match_recove_tos : BaseToSC
	{
		public override int __ID(){ return 1405; }
	}

	//战斗结束统一接口
	public class m_fight_finish_tos : BaseToSC
	{
		public Int32 fight_type = 0;	//required	int32	关卡类型

		public string miss_packet = "";	//optional   string  操作(帧)

		public p_kvi[] para = null;	//repeated 	p_kvi	特殊数据

		public override int __ID(){ return 1409; }
	}

	public class m_fight_exit_fight_tos : BaseToSC
	{
		public override int __ID(){ return 1410; }
	}

	public class m_fight_change_tcp_tos : BaseToSC
	{
		public Int32 type = 1;	//required int32  1 udp切tcp 2 tcp 切UDP

		public Int32 frame_count = 0;	//required int32

		public override int __ID(){ return 1411; }
	}

	public class m_fight_kill_info_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32 1 英雄 2小怪 3防御塔 4野怪小怪 5野怪boss 6无主小怪掉落

		public Int32 kill_key = 0;	//required int32  怪物ID或者帧数*10+控制序列

		public long kill_roleid = 0;	//required int64	击杀者，0表示不是玩家

		public long[] role_ids = null;	//repeated int64	其他相关玩家，如助攻者、在范围内的人

		public long be_kill_role_id = 0;	//required int64	被击杀的玩家id

		public p_kvi[] para = null;	//repeated p_kvi	其他相关参数 1连杀数，连胜正数，被连杀负数  2 击杀小怪的类型 1近战2远程3空军4精英 3防御塔类型（1外塔，2内塔）

		public override int __ID(){ return 1416; }
	}

	public class m_fight_skill_levelup_tos : BaseToSC
	{
		public Int32 skill_id = 0;	//required int32

		public Int32 ex_skill_id = 0;	//required int32

		public Int32 level = 0;	//required int32

		public Int32[] list = null;	//repeated int32

		public override int __ID(){ return 1415; }
	}

	public class m_fight_udp_pack_tos : BaseToSC
	{
		public byte[] data = null;	//repeated byte

		public override int __ID(){ return 1412; }
	}

	public class m_fight_fail_apply_tos : BaseToSC
	{
		public override int __ID(){ return 1417; }
	}

	public class m_fight_fail_result_tos : BaseToSC
	{
		public Int32 type = 1;	//required int32 1同意 2拒绝

		public override int __ID(){ return 1418; }
	}

	//加速验证
	public class m_fight_time_check_tos : BaseToSC
	{
		public Int32 fight_seq = 0;	//required	int32

		public Int32 time = 0;	//optional   int32  用时

		public override int __ID(){ return 1414; }
	}

	//加速验证
	public class m_stage_time_check_tos
	{
		public Int32 fight_seq = 0;	//required	int32

		public Int32 time = 0;	//optional   int32  用时

	}

	public class m_common_lottery_tos : BaseToSC
	{
		public Int32 key = 0;	//required	int32	%返回值

		public override int __ID(){ return 1602; }
	}

	public class m_common_client_update_tos : BaseToSC
	{
		public override int __ID(){ return 1603; }
	}

	public class m_common_update_player_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32		1.fps

		public Int32 value = 0;	//required int32

		public override int __ID(){ return 1606; }
	}

	//领奖励
	public class m_login_award_get_tos : BaseToSC
	{
		public Int32 award_type = 0;	//required int32  		1.七日奖励 2回归奖励， 3周一惊喜

		public Int32 days = 0;	//optional int32  		领第几天的

		public override int __ID(){ return 1703; }
	}

	//查询媒体卡活动
	public class m_festival_card_get_tos : BaseToSC
	{
		public override int __ID(){ return 1818; }
	}

	//媒体卡兑换
	public class m_festival_card_exchange_tos : BaseToSC
	{
		public string card_num = "";	//required string  卡号

		public override int __ID(){ return 1808; }
	}

	//F码查询
	public class m_festival_fcode_get_info_tos : BaseToSC
	{
		public override int __ID(){ return 1810; }
	}

	//F码兑换
	public class m_festival_fcode_exchange_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32		1:领取  2：金币过期  3：糖果过期

		public string fcode = "";	//required string	F码

		public string user_name = "";	//required string	平台用户名

		public string app_version = "";	//required string	版本号

		public string server_name = "";	//required string	游戏服名字

		public override int __ID(){ return 1809; }
	}

	public class m_festival_get_info_tos : BaseToSC
	{
		public override int __ID(){ return 1802; }
	}

	public class m_festival_get_award_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public Int32 key = 0;	//required int32

		public override int __ID(){ return 1803; }
	}

	public class m_festival_slot_get_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public override int __ID(){ return 1811; }
	}

	public class m_festival_slot_start_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32 活动id

		public Int32 index = 0;	//required int32 目前先填1

		public Boolean is_buy = false;	//required bool 是否购买次数

		public override int __ID(){ return 1812; }
	}

	public class m_festival_slot_get_award_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public Int32 key = 0;	//required int32

		public override int __ID(){ return 1813; }
	}

	public class m_festival_slot_use_item_tos : BaseToSC
	{
		public Int32 type_id = 0;	//required int32  物品id

		public override int __ID(){ return 1814; }
	}

	public class m_festival_lucky_bag_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32	活动id

		public Int32 type = 0;	//required int32	类型 1:一次  2:十次

		public override int __ID(){ return 1815; }
	}

	public class m_festival_lucky_bag_rank_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32	活动id

		public override int __ID(){ return 1816; }
	}

	public class m_festival_pay_get_rank_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32	活动id

		public override int __ID(){ return 1817; }
	}

	public class m_festival_snow_use_item_tos
	{
		public long role_id = 0;	//required	int64 	角色ID

	}

	public class m_festival_snow_hide_tos
	{
	}

	public class m_festival_card_send_roles_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32 活动id

		public override int __ID(){ return 1819; }
	}

	public class m_festival_card_use_tos : BaseToSC
	{
		public Int32 id = 0;	//required	int32	活动id

		public Int32 goods_id = 0;	//required   int32	使用的物品ID

		public long role_id = 0;	//required	int64	赠送的对象

		public string text = "";	//required	string  正文

		public override int __ID(){ return 1820; }
	}

	public class m_festival_card_show_tos : BaseToSC
	{
		public Int32 id = 0;	//required	int32	活动id

		public override int __ID(){ return 1821; }
	}

	public class m_festival_card_get_info_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32	活动id

		public Int32 page_id = 1;	//required int32	页数

		public Int32 page_size = 6;	//required int32 每页条数

		public override int __ID(){ return 1822; }
	}

	public class m_festival_card_read_tos : BaseToSC
	{
		public Int32 id = 0;	//required	int32	活动id

		public Int32 seq = 0;	//required	int32	贺卡id

		public override int __ID(){ return 1824; }
	}

	public class m_festival_card_delete_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32	活动id

		public Int32 seq = 0;	//required int32	贺卡id 0表示删除所有已读的贺卡

		public Int32 page_size = 6;	//required int32	每页条数

		public Int32 page_id = 1;	//required int32	第几页

		public override int __ID(){ return 1825; }
	}

	public class m_festival_egg_tos : BaseToSC
	{
		public Int32 festival_id = 0;	//required int32 活动ID

		public override int __ID(){ return 1826; }
	}

	public class m_festival_egg_open_tos : BaseToSC
	{
		public Int32 festival_id = 0;	//required int32

		public Int32 index = 0;	//required int32

		public override int __ID(){ return 1827; }
	}

	//创建队列
	public class m_pvp5_create_queue_tos : BaseToSC
	{
		public Int32 type = 1;	//required  int32  1、对抗赛 2、排位赛 3、练习赛入门  4、练习赛简单 5、练习赛一般 6自定义

		public override int __ID(){ return 1902; }
	}

	//邀请好友
	public class m_pvp5_invite_tos : BaseToSC
	{
		public long role_id = 0;	//required  int64

		public string role_name = "";	//required  string,

		public Int32 type = 1;	//optional  int32  1、对抗赛 2、排位赛 3、练习赛入门  4、练习赛简单 5、练习赛一般 6自定义

		public override int __ID(){ return 1903; }
	}

	//处理邀请
	public class m_pvp5_invite_result_tos : BaseToSC
	{
		public long invite_roleid = 0;	//required  int64

		public Int32 type = 1;	//optional  int32  1、对抗赛 2、排位赛 3、练习赛入门  4、练习赛简单 5、练习赛一般 6自定义

		public Boolean is_agree = false;	//required  bool

		public override int __ID(){ return 1904; }
	}

	//踢人
	public class m_pvp5_kick_tos : BaseToSC
	{
		public long role_id = 0;	//required  int64

		public override int __ID(){ return 1905; }
	}

	//退出
	public class m_pvp5_quit_tos : BaseToSC
	{
		public override int __ID(){ return 1906; }
	}

	//开始匹配
	public class m_pvp5_match_tos : BaseToSC
	{
		public string[] machine_name = null;	//repeated string 机器人名字

		public override int __ID(){ return 1907; }
	}

	//取消匹配
	public class m_pvp5_cancel_match_tos : BaseToSC
	{
		public override int __ID(){ return 1908; }
	}

	//战斗确认
	public class m_pvp5_fight_confirm_tos : BaseToSC
	{
		public Int32 type = 1;	//required  int32   匹配模式  1对抗赛，2排位赛

		public override int __ID(){ return 1910; }
	}

	//选择英雄技能等
	public class m_pvp5_hero_select_tos : BaseToSC
	{
		public Int32 state = 0;	//required  int32 1确认英雄，0未确认

		public Int32 hero_type = 0;	//required  int32

		public Int32 type = 0;	//required  int32   匹配模式  1对抗赛，2排位赛

		public Int32[] skill_list = null;	//repeated  int32

		public Int32 chip_id = 0;	//required  int32

		public Int32 skin_id = 0;	//required  int32

		public override int __ID(){ return 1912; }
	}

	public class m_pvp5_get_result_tos : BaseToSC
	{
		public override int __ID(){ return 1915; }
	}

	public class m_pvp5_match_rank_info_tos : BaseToSC
	{
		public override int __ID(){ return 1922; }
	}

	public class m_pvp5_single_fight_result_tos : BaseToSC
	{
		public Boolean win = false;	//required   bool     是否获胜

		public Int32 time = 0;	//required   int32 	 总时间，单位秒

		public p_fight_result[] fight_result = null;	//repeated	p_fight_result	本场战斗结果

		public override int __ID(){ return 1923; }
	}

	//自定义比赛换边
	public class m_pvp5_diy_chg_tos : BaseToSC
	{
		public Int32 type = 1;	//required  int32 1换到下面，2换到上面

		public override int __ID(){ return 1917; }
	}

	//自定义比赛加机器人
	public class m_pvp5_diy_add_tos : BaseToSC
	{
		public Int32 type = 1;	//required  int32 1加到上面 2加到下面

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public override int __ID(){ return 1918; }
	}

	//自定义比赛开始
	public class m_pvp5_diy_start_tos : BaseToSC
	{
		public override int __ID(){ return 1919; }
	}

	//自定义解散
	public class m_pvp5_diy_disband_tos : BaseToSC
	{
		public override int __ID(){ return 1920; }
	}

	//举报
	public class m_pvp5_report_tos : BaseToSC
	{
		public Int32 fight_id = 0;	//required int32

		public long dest_role = 0;	//required int64

		public override int __ID(){ return 1924; }
	}

	public class m_pvp5_dian_zan_tos : BaseToSC
	{
		public override int __ID(){ return 1926; }
	}

	//匹配结束%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	public class m_fcm_set_tos : BaseToSC
	{
		public override int __ID(){ return 2002; }
	}

	//客户端更新状态
	public class m_story_update_tos : BaseToSC
	{
		public Int32 key = 0;	//required int32

		public Int32 value = 0;	//required int32

		public override int __ID(){ return 2103; }
	}

	public class m_story_sprog_update_tos : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public override int __ID(){ return 2104; }
	}

	//查询
	public class m_treasure_box_get_tos : BaseToSC
	{
		public override int __ID(){ return 2202; }
	}

	//解锁
	public class m_treasure_box_deblocking_tos : BaseToSC
	{
		public Int32 pos_id = 0;	//required	int32

		public override int __ID(){ return 2203; }
	}

	//打开
	public class m_treasure_box_open_tos : BaseToSC
	{
		public Int32 pos_id = 0;	//required	int32

		public Int32 type = 0;	//required	int32 1 钻 2 碎片

		public override int __ID(){ return 2204; }
	}

	public class m_sweet_share_tos : BaseToSC
	{
		public override int __ID(){ return 2302; }
	}

	public class m_sweet_buy_tos : BaseToSC
	{
		public override int __ID(){ return 2303; }
	}

	public class m_sweet_qry_tos : BaseToSC
	{
		public override int __ID(){ return 2304; }
	}

	public class m_sweet_fight_award_tos : BaseToSC
	{
		public override int __ID(){ return 2305; }
	}

	public class m_login_award_pay_first_award_tos : BaseToSC
	{
		public override int __ID(){ return 1706; }
	}

	//获得合金（月卡、周卡、每天登陆）
	public class m_login_award_alloy_award_tos : BaseToSC
	{
		public Int32 type = 0;	//required	int32 1是每天登陆领取、2是月卡、3是周卡

		public override int __ID(){ return 1707; }
	}

	//---------------------------egg-----------------------------------------
	public class m_egg_get_tos : BaseToSC
	{
		public override int __ID(){ return 2602; }
	}

	public class m_egg_open_tos : BaseToSC
	{
		public Int32 index = 0;	//required int32 要开启的位置

		public override int __ID(){ return 2603; }
	}

	//-----------------------------------------------------------------------
	//--------------------------treasure-----------------------------------------
	//获取面板数据
	public class m_treasure_get_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32 1:原石，2：合金

		public override int __ID(){ return 2702; }
	}

	//点击夺宝
	public class m_treasure_buy_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32

		public Int32 num_type = 0;	//required int32 1:1次，2:5次

		public override int __ID(){ return 2703; }
	}

	//领取次数奖励
	public class m_treasure_get_award_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32

		public Int32 id = 0;	//required int32

		public override int __ID(){ return 2704; }
	}

	public class m_chip_qry_tos : BaseToSC
	{
		public override int __ID(){ return 2402; }
	}

	public class m_chip_chg_tos : BaseToSC
	{
		public Int32 page_id = 0;	//required    int32   芯片页码

		public override int __ID(){ return 2408; }
	}

	public class m_chip_load_tos : BaseToSC
	{
		public Int32 page_id = 0;	//required    int32   芯片页码

		public Int32 pos_id = 0;	//required    int32   位置 1到30

		public Int32 id = 0;	//required    int32

		public override int __ID(){ return 2403; }
	}

	public class m_chip_unload_tos : BaseToSC
	{
		public Int32 page_id = 0;	//required    int32   芯片页码

		public Int32 pos_id = 0;	//required    int32   位置 1到30 0时表示全部拆卸

		public override int __ID(){ return 2404; }
	}

	public class m_chip_open_tos : BaseToSC
	{
		public Int32 page_id = 0;	//required    int32   芯片页码

		public override int __ID(){ return 2405; }
	}

	public class m_chip_buy_tos : BaseToSC
	{
		public Int32 type_id = 0;	//required    int32

		public Int32 num = 0;	//required    int32

		public override int __ID(){ return 2406; }
	}

	public class m_chip_sell_tos : BaseToSC
	{
		public Int32[] ids = null;	//repeated    int32   出售id列表

		public override int __ID(){ return 2407; }
	}

	public class m_chip_chg_name_tos : BaseToSC
	{
		public Int32 page_id = 0;	//required    int32   芯片页码

		public string page_name = "";	//required    string

		public override int __ID(){ return 2410; }
	}

	public class m_chip_lottery_tos : BaseToSC
	{
		public Int32 type = 0;	//required    int32   1金币 2合金

		public Int32 times = 0;	//required    int32	 次数

		public override int __ID(){ return 2411; }
	}

	public class m_chip_open_slot_tos : BaseToSC
	{
		public Int32 pos_id = 0;	//required    int32   位置

		public override int __ID(){ return 2412; }
	}

	//---------------------------------------------------------------------------------
	public class m_vip_get_award_info_tos : BaseToSC
	{
		public override int __ID(){ return 2502; }
	}

	public class m_vip_take_award_tos : BaseToSC
	{
		public Int32 type = 0;	//required int32   1是免费，2是买

		public Int32 vip_grade = 0;	//required int32  vip等级

		public override int __ID(){ return 2503; }
	}

}

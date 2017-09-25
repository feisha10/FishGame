using System;
using System.Collections.Generic;

namespace Client
{
	public class m_client_login_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	是否验证成功

		public p_msg reason = null;	//optional p_msg

		public string account = "";	//optional	string	账号,平台认证成功后返回

		public Boolean has_role = false;	//optional	bool	是否已有角色。如果有，会立即进入游戏；如果没有，需要创建角色。

		public p_account_role[] roles = null;	//repeated	p_account_role	%角色列表

		public long last_role = 0;	//optional	int64	%上次登陆角色

		public Int32 time = 0;	//optional	int32	服务器当前时间戳

		public string ext = "";	//optional	string 平台返回的原串

		public override int __ID(){ return 202; }
	}

	public class m_client_select_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	是否验证成功

		public p_msg reason = null;	//optional p_msg

		public p_account_role[] roles = null;	//repeated	p_account_role	%角色列表

		public long last_role = 0;	//optional	int64	%上次登陆角色

		public override int __ID(){ return 215; }
	}

	public class m_client_close_toc : BaseToSC
	{
		public Int32 type = 0;	//required	int32	断开原因

		public override int __ID(){ return 216; }
	}

	public class m_client_create_role_name_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool

		public p_msg reason = null;	//optional p_msg

		public string[] role_name_list = null;	//repeated	string

		public override int __ID(){ return 223; }
	}

	public class m_client_create_role_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	如果成功，会立即进入游戏

		public p_msg reason = null;	//optional p_msg

		public long role_id = 0;	//optional	int64

		public string role_name = "";	//optional	string

		public override int __ID(){ return 203; }
	}

	public class m_client_enter_game_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	如果成功，会立即进入游戏

		public p_msg reason = null;	//optional p_msg

		public long role_id = 0;	//optional	int64

		public p_role_base role_base = null;	//optional	p_role_base

		public p_role_attr role_attr = null;	//optional	p_role_attr

		public p_hero[] heros = null;	//repeated   p_hero

		public Int32 win_first_time = 0;	//optional   int32   下次首胜时间戳 0或者小于当前是可以获得首胜了

		public Int32 udp_port = 0;	//optional   int32

		public Int32[] week_heros = null;	//repeated   int32

		public override int __ID(){ return 204; }
	}

	public class m_client_exe_cmd_toc : BaseToSC
	{
		public Boolean exeok = true;	//required	bool

		public string result = "";	//required	string

		public override int __ID(){ return 205; }
	}

	public class m_client_logout_toc : BaseToSC
	{
		public override int __ID(){ return 207; }
	}

	public class m_client_get_role_icon_toc : BaseToSC
	{
		public p_role_view_icon[] view_list = null;	//repeated	p_role_view_icon

		public override int __ID(){ return 206; }
	}

	public class m_client_bind_account_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public override int __ID(){ return 209; }
	}

	public class m_client_gm_complaint_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public override int __ID(){ return 210; }
	}

	public class m_client_reconnect_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public override int __ID(){ return 211; }
	}

	// 主动推送，通知属性改变
	public class m_client_attr_change_toc : BaseToSC
	{
		public p_kvi[] data = null;	//repeated	p_kvi  %%key:属性索引，见 common.hrl 的 ATT_XX , value:	属性数值,仅支持整形

		public override int __ID(){ return 208; }
	}

	// 主动推送，通知属性改变只支持int32类型的修改
	public class m_client_base_change_toc : BaseToSC
	{
		public p_kvi[] data = null;	//repeated	p_kvi  %%key:属性索引，value:	属性数值,仅支持整形

		public override int __ID(){ return 222; }
	}

	//属性变化，通知接口
	public class m_client_props_change_toc : BaseToSC	//cross
	{
		public Int32 hero_type = 0;	//required  int32

		public p_prop props_change = null;	//optional	p_prop

		public Dictionary<string, object> kvs = new Dictionary<string,object>(); //生成的，kv列表
		public override int __ID(){ return 212; }
	}

	//附加属性变化 通知接口
	public class m_client_props_add_change_toc : BaseToSC
	{
		public Int32 hero_type = 0;	//required  int32

		public p_kvi[] change = null;	//repeated	p_kvi

		public override int __ID(){ return 213; }
	}

	public class m_client_login_notice_toc : BaseToSC
	{
		public Boolean first_login = false;	//required bool 是否今天第一次登入

		public p_login_notice[] notice_list = null;	//repeated p_login_notice 登入公告列表

		public override int __ID(){ return 214; }
	}

	public class m_client_unlink_account_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public override int __ID(){ return 220; }
	}

	//RPC客户端处理
	public class m_client_do_client_cmd_toc : BaseToSC
	{
		public string str_cmd = "";	//required string    %命令字符串

		public override int __ID(){ return 217; }
	}

	public class m_client_hide_toc : BaseToSC
	{
		public override int __ID(){ return 218; }
	}

	public class m_client_tst_card_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional   p_msg	操作失败原因

		public override int __ID(){ return 219; }
	}

	public class m_client_day_notice_toc : BaseToSC
	{
		public Int32 days = 0;	//required int32  开服天数

		public Int32 level = 0;	//optional int32  服务器等级

		public override int __ID(){ return 221; }
	}

	public class m_bag_get_bag_info_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public p_goods[] goods_list = null;	//repeated   p_goods 背包内物品

		public override int __ID(){ return 302; }
	}

	public class m_bag_update_notice_toc : BaseToSC
	{
		public p_goods[] goods_list = null;	//repeated   p_goods 更新背包内物品，只包含改变了的

		public Int32 type = 0;	//required	int32  0忽略,1盗贼 2使用伙伴道具获得契约

		public override int __ID(){ return 304; }
	}

	public class m_bag_sell_goods_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public Int32 goods_id = 0;	//optional   int32   出售的物品ID

		public Int32 num = 0;	//optional   int32   出售物品剩余数量,如果卖完则为0

		public override int __ID(){ return 303; }
	}

	public class m_bag_buy_back_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public p_goods[] goods_list = null;	//repeated   p_goods

		public Int32 id = 0;	//optional int32

		public override int __ID(){ return 305; }
	}

	public class m_bag_compose_goods_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool 操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public p_goods[] goods = null;	//repeated p_goods 更新背包物品

		public Int32 succ_num = 0;	//optional int32 成功次数

		public Int32 fail_num = 0;	//optional int32 失败次数

		public override int __ID(){ return 306; }
	}

	public class m_bag_dec_goods_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] goods = null;	//repeated p_goods

		public override int __ID(){ return 307; }
	}

	public class m_bag_multiple_compose_goods_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] goods = null;	//repeated p_goods

		public override int __ID(){ return 308; }
	}

	public class m_bag_exchange_money_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public override int __ID(){ return 309; }
	}

	public class m_item_use_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public Int32 goods_id = 0;	//optional    int32   使用的物品ID

		public Int32 rest_num = 0;	//optional    int32   剩余数量

		public p_kvi[] goods = null;	//repeated   p_kvi	获得的物品， key是type_id, value 是个数

		public override int __ID(){ return 402; }
	}

	public class m_shop_shops_list_toc : BaseToSC
	{
		public p_shop_info[] shops = null;	//repeated  p_shop_info

		public p_shop_buy_info[] sell_info = null;	//repeated  p_shop_buy_info

		public override int __ID(){ return 602; }
	}

	public class m_shop_get_goods_toc : BaseToSC
	{
		public Int32 shop_id = 0;	//required int32  所属商店

		public Int32 npc_id = 0;	//optional int32  npc_id 0表示商城

		public p_shop_goods_info[] all_goods = null;	//repeated p_shop_goods_info  该商店所有商品的信息

		public override int __ID(){ return 603; }
	}

	public class m_shop_buy_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool 操作成功

		public p_msg reason = null;	//optional p_msg 操作失败原因

		public Int32 shop_id = 0;	//optional int32  商店id

		public p_shop_goods_info goods_info = null;	//optional p_shop_goods_info  该商店所有商品的信息

		public p_goods[] update_goods = null;	//repeated p_goods

		public p_chip_info[] chip_list = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 604; }
	}

	public class m_shop_dial_info_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool

		public p_msg reason = null;	//optional	p_msg

		public p_dial[] shop_list = null;	//repeated	p_dial	物品id列表及个数

		public override int __ID(){ return 605; }
	}

	public class m_shop_buy_dial_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool

		public p_msg reason = null;	//optional	p_msg

		public p_client_good award = null;	//optional	p_client_good

		public p_goods[] update_goods = null;	//repeated p_goods 更新背包

		public Int32 index = 0;	//optional	int32	第几个

		public override int __ID(){ return 606; }
	}

	public class m_shop_get_goods_by_type_id_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool

		public p_msg reason = null;	//optional	p_msg

		public Int32 shop_id = 0;	//optional int32  所属商店

		public p_shop_goods_info goods = null;	//optional p_shop_goods_info  该商店所有商品的信息

		public override int __ID(){ return 607; }
	}

	public class m_shop_pay_buy_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool 操作成功

		public p_msg reason = null;	//optional p_msg 操作失败原因

		public Int32 shop_id = 0;	//optional int32  商店id

		public p_shop_goods_info goods_info = null;	//optional p_shop_goods_info  该商店所有商品的信息

		public p_goods[] update_goods = null;	//repeated p_goods

		public p_chip_info[] chip_list = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 608; }
	}

	public class m_letter_send_letter_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool	操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public override int __ID(){ return 502; }
	}

	public class m_letter_new_letter_toc : BaseToSC
	{
		public p_letter new_letter = null;	//required p_letter 	新邮件

		public override int __ID(){ return 503; }
	}

	public class m_letter_new_count_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool		操作成功

		public p_msg reason = null;	//optional p_msg	操作失败原因

		public Int32 new_count = 0;	//optional int32 	新邮件数量

		public override int __ID(){ return 508; }
	}

	public class m_letter_open_mailbox_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool		操作成功

		public p_msg reason = null;	//optional p_msg		操作失败原因

		public Int32 page_id = 0;	//optional int32		第几页

		public p_letter[] recv_letters = null;	//repeated p_letter 	接收邮件列表

		public Int32 total_num = 0;	//optional int32		邮件总数

		public override int __ID(){ return 504; }
	}

	public class m_letter_delete_letter_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool		操作成功

		public p_msg reason = null;	//optional p_msg		操作失败原因

		public Int32 seq = 0;	//optional int32 	删除邮件seq

		public Int32 page_id = 0;	//optional int32		第几页

		public p_letter[] recv_letters = null;	//repeated p_letter 	接收邮件列表

		public Int32 total_num = 0;	//optional int32		邮件总数

		public override int __ID(){ return 506; }
	}

	public class m_letter_read_letter_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool	操作成功

		public p_msg reason = null;	//optional p_msg 操作失败原因

		public Int32 seq = 0;	//optional int32 读取邮件seq

		public override int __ID(){ return 505; }
	}

	public class m_letter_get_attach_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool 操作成功

		public p_msg reason = null;	//optional p_msg 操作失败原因

		public p_award[] common_awards = null;	//repeated p_award 奖励

		public Int32 seq = 0;	//optional int32 删除邮件seq

		public override int __ID(){ return 507; }
	}

	public class m_letter_get_all_attach_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool 操作成功

		public p_msg reason = null;	//optional p_msg 操作失败原因

		public p_goods[] common_awards = null;	//repeated p_goods 奖励

		public Int32[] seq = null;	//repeated int32 删除邮件seq

		public Int32 type = 0;	//optional int32 操作结果 0没有可取的邮件,1全部取完,2部分取完(背包空间已满)

		public override int __ID(){ return 509; }
	}

	public class m_task_update_toc : BaseToSC
	{
		public Int32 task_id = 0;	//required int32   任务id

		public Int32 progress = 0;	//required int32  更新后的进度（该协议有可能主动推送，用来更新任务）

		public override int __ID(){ return 702; }
	}

	public class m_task_accept_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_task task = null;	//optional p_task  接受的任务数据

		public override int __ID(){ return 703; }
	}

	public class m_task_finish_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_task_finish task_finish = null;	//optional p_task_finish

		public p_goods[] goods = null;	//repeated p_goods 更新背包

		public override int __ID(){ return 704; }
	}

	public class m_task_freshen_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 task_id = 0;	//optional int32

		public p_task task_info = null;	//optional p_task

		public override int __ID(){ return 706; }
	}

	public class m_task_all_toc : BaseToSC
	{
		public p_task[] task_list = null;	//repeated p_task    	已经接受了的任务

		public p_task_finish[] finish_list = null;	//repeated p_task_finish 已经完成任务

		public Int32 time = 0;	//required int32			当前时间戳

		public Int32 group_id = 0;	//optional int32    成长任务当前的组别

		public Int32 star = 0;	//optional int32    成长任务星数

		public Int32[] award_list = null;	//repeated int32 	成长任务已领奖励星数列表

		public override int __ID(){ return 705; }
	}

	public class m_task_star_award_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] goods_list = null;	//repeated p_goods 更新背包

		public Int32 star = 0;	//optional int32

		public override int __ID(){ return 707; }
	}

	public class m_task_star_chg_toc : BaseToSC
	{
		public Int32 group_id = 0;	//optional int32    成长任务当前的组别

		public p_task[] task_list = null;	//repeated p_task   新group_id接受的任务

		public Int32 star = 0;	//optional int32    成长任务星数

		public override int __ID(){ return 708; }
	}

	//--------------------------------------hero----------------------------------------
	public class m_hero_add_exp_toc : BaseToSC
	{
		public Int32 now_exp = 0;	//required int32		当前经验（如果要显示加了多少，客户端-一下）

		public override int __ID(){ return 802; }
	}

	public class m_hero_level_up_toc : BaseToSC
	{
		public Int32 level = 0;	//required int32		等级

		public Int32 now_exp = 0;	//required int32		当前经验

		public Int32 add_exp = 0;	//required int32		增加的经验

		public override int __ID(){ return 803; }
	}

	public class m_hero_get_heros_toc : BaseToSC
	{
		public p_hero[] heros = null;	//repeated 	p_hero 	英雄

		public override int __ID(){ return 804; }
	}

	public class m_hero_head_upload_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public string addr = "";	//optional string	头像地址

		public Boolean is_set_null = false;	//optional bool	头像地址

		public override int __ID(){ return 805; }
	}

	public class m_hero_headstr_tmp_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public string addr = "";	//optional string	头像地址

		public override int __ID(){ return 815; }
	}

	public class m_hero_headframe_notice_toc : BaseToSC
	{
		public Int32[] head_frame_list = null;	//repeated int32

		public override int __ID(){ return 816; }
	}

	public class m_hero_use_headframe_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 head_frame_id = 0;	//optional int32

		public override int __ID(){ return 817; }
	}

	public class m_hero_sign_str_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public override int __ID(){ return 806; }
	}

	public class m_hero_notice_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32   1 获得 2延期 3转化为合金

		public Int32 hero_type = 0;	//optional int32  英雄id

		public Int32 valid_day = 0;	//optional int32   天数， -1表示永久

		public Int32 add_gold = 0;	//optional int32   转化的合金

		public override int __ID(){ return 807; }
	}

	public class m_hero_skin_notice_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32   1 获得 2延期 3转化为碎片

		public Int32 hero_type = 0;	//optional int32  英雄id

		public Int32 skin_id = 0;	//optional int32  皮肤ID

		public Int32 valid_day = 0;	//optional int32   天数， -1表示永久

		public Int32 num = 0;	//optional int32   转化为皮肤碎片的个数

		public override int __ID(){ return 808; }
	}

	public class m_hero_chg_skin_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 hero_type = 0;	//optional int32  英雄id

		public Int32 skin_id = 0;	//optional int32  皮肤ID

		public Int32 old_skin_id = 0;	//optional int32	原来的皮肤ID

		public override int __ID(){ return 809; }
	}

	public class m_hero_equip_chg_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 hero_type = 0;	//optional int32  英雄id

		public p_equip_info equip_info = null;	//optional p_equip_info

		public Int32 type = 0;	//optional int32  前端用

		public override int __ID(){ return 810; }
	}

	public class m_hero_get_roleinfo_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public long role_id = 0;	//required	int64	角色id

		public string role_name = "";	//optional	string	角色名

		public string head_addr = "";	//optional	string	头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 vip_grade = 0;	//optional int32	vip等级

		public string sign_str = "";	//optional	string	个性签名

		public Int32 level = 0;	//optional   int32

		public Int32 dan_grading = 1;	//optional   int32   段位

		public Int32 high_grading = 1;	//optional   int32   历史最高段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 hero_num = 0;	//optional	int32  	英雄数量

		public Int32 skin_num = 0;	//optional	int32  	皮肤数量

		public p_pvp5_fight_result[] result_list = null;	//repeated   p_pvp5_fight_result

		public p_fight_info pvp5_info = null;	//optional   p_fight_info

		public p_fight_info week_pvp5_info = null;	//optional   p_fight_info

		public p_fight_info rank_info = null;	//optional   p_fight_info

		public p_fight_info week_rank_info = null;	//optional   p_fight_info

		public p_fight_hero_info[] pvp5_hero_list = null;	//repeated   p_fight_hero_info

		public p_fight_hero_info[] rank_hero_list = null;	//repeated   p_fight_hero_info

		public Int32 achieve_num = 0;	//optional    int32 完成成就个数

		public Int32 allow_qry = 0;	//optional	int32	用来表示是否允许别人查看， 0表示允许， 1表示不允许

		public long corps_id = 0;	//required	int64   公会id

		public string corps_name = "";	//required	string   公会名称

		public Int32 corps_office = 0;	//optional	int32   公会官职

		public override int __ID(){ return 811; }
	}

	public class m_hero_set_qry_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 0;	//optional int32

		public override int __ID(){ return 812; }
	}

	public class m_hero_weekfree_chg_toc : BaseToSC
	{
		public Int32[] week_free_list = null;	//repeated  int32

		public override int __ID(){ return 813; }
	}

	public class m_hero_change_name_toc : BaseToSC
	{
		public Boolean succ = true;	//required bool

		public p_msg reason = null;	//optional p_msg

		public string role_name = "";	//optional string

		public p_goods[] goods_list = null;	//repeated p_goods %%更新背包

		public override int __ID(){ return 814; }
	}

	public class m_rank_get_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 rank_time = 0;	//optional int32	下次重排时间

		public Int32 rank_id = 0;	//optional int32	排行榜ID

		public p_ranking_role_info my_rank = null;	//optional p_ranking_role_info	我的排行，0是未上榜

		public Int32 page_id = 0;	//optional int32	第几页

		public Int32 page_count = 0;	//optional int32	总页数

		public p_ranking_role_info[] rank_list = null;	//repeated p_ranking_role_info

		public override int __ID(){ return 902; }
	}

	public class m_rank_level_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public Int32 rank_time = 0;	//optional int32	下次重排时间

		public Int32 my_rank = 0;	//optional int32	我的排行，0是未上榜

		public Int32 page_id = 0;	//optional int32	第几页

		public Int32 page_count = 0;	//optional int32	总页数

		public p_ranking_role_level[] rank_list = null;	//repeated p_ranking_role_level

		public override int __ID(){ return 904; }
	}

	public class m_friend_qry_role_toc : BaseToSC
	{
		public p_friend_info[] role_list = null;	//repeated p_friend_info

		public override int __ID(){ return 1006; }
	}

	public class m_friend_querry_info_toc : BaseToSC
	{
		public p_friend_info[] info_list = null;	//repeated p_friend_info 好友列表

		public p_role_friend_request[] request_list = null;	//repeated p_role_friend_request

		public override int __ID(){ return 1002; }
	}

	public class m_friend_querry_rank_toc : BaseToSC
	{
		public p_friend_rank_info[] info_list = null;	//repeated p_friend_rank_info

		public override int __ID(){ return 1011; }
	}

	public class m_friend_add_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public p_role_friend_request friend_info = null;	//optional p_role_friend_request 好友信息

		public override int __ID(){ return 1003; }
	}

	public class m_friend_add_result_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public Boolean is_agree = false;	//optional  bool

		public long role_id = 0;	//optional int64 玩家id

		public p_friend_info info = null;	//optional  p_friend_info

		public override int __ID(){ return 1004; }
	}

	public class m_friend_delete_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long role_id = 0;	//optional int64 玩家id

		public string role_name = "";	//optional  string

		public override int __ID(){ return 1005; }
	}

	public class m_friend_state_notice_toc : BaseToSC
	{
		public long role_id = 0;	//optional int64 玩家id

		public Int32 state = 0;	//optional  int32

		public Int32 state_time = 0;	//optional  int32

		public override int __ID(){ return 1007; }
	}

	//服务端主动推送
	public class m_friend_notice_new_friend_toc
	{
		public p_friend_info[] friends = null;	//repeated p_friend_info 好友信息

	}

	public class m_friend_near_get_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_near_info[] list = null;	//repeated p_near_info

		public override int __ID(){ return 1008; }
	}

	public class m_friend_near_chg_state_toc : BaseToSC
	{
		public long role_id = 0;	//required	int64

		public Int32 state = 0;	//optional	int32

		public override int __ID(){ return 1009; }
	}

	public class m_corps_create_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public long corps_id = 0;	//optional int64

		public string corps_name = "";	//optional string

		public override int __ID(){ return 1302; }
	}

	public class m_corps_list_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_corps_summary[] corps_list = null;	//repeated p_corps_summary

		public Int32 num = 0;	//optional int32    战队总数

		public Int32 page_id = 0;	//optional int32   第几页

		public long[] request_list = null;	//repeated int64

		public override int __ID(){ return 1303; }
	}

	public class m_corps_rank_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_corps_summary[] corps_list = null;	//repeated p_corps_summary

		public override int __ID(){ return 1328; }
	}

	public class m_corps_qry_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public string key = "";	//optional string

		public p_corps_summary[] corps = null;	//repeated p_corps_summary

		public override int __ID(){ return 1304; }
	}

	public class m_corps_getinfo_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_corps_info corps = null;	//optional p_corps_info

		public override int __ID(){ return 1314; }
	}

	public class m_corps_sign_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public override int __ID(){ return 1305; }
	}

	public class m_corps_request_list_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_corps_role_request[] list = null;	//repeated p_corps_role_request

		public override int __ID(){ return 1306; }
	}

	public class m_corps_request_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_agree = false;	//optional  bool

		public long[] request_list = null;	//repeated int64

		public long corps_id = 0;	//optional int64

		public string corps_name = "";	//optional string

		public override int __ID(){ return 1307; }
	}

	public class m_corps_request_all_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public long[] request_list = null;	//repeated int64

		public long corps_id = 0;	//optional int64

		public string corps_name = "";	//optional string

		public override int __ID(){ return 1308; }
	}

	public class m_corps_accept_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//required bool

		public Int32 accept_type = 0;	//optional int32  1同意加入，2拒绝，3清空

		public long corps_id = 0;	//optional int64

		public string corps_name = "";	//optional string

		public p_corps_member_info[] members = null;	//repeated p_corps_member_info

		public override int __ID(){ return 1309; }
	}

	public class m_corps_change_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 0;	//required int32 1修改入会条件， 2修改公告

		public p_kvi[] join_condition = null;	//repeated p_kvi

		public string notice = "";	//optional string

		public override int __ID(){ return 1310; }
	}

	public class m_corps_chg_agree_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean auto_agree = false;	//optional bool 自动审批

		public override int __ID(){ return 1319; }
	}

	public class m_corps_quit_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//required bool

		public long role_id = 0;	//optional int64 退出的人

		public override int __ID(){ return 1311; }
	}

	public class m_corps_kick_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//required bool

		public long[] role_ids = null;	//repeated int64 %%不能踢的人

		public long role_id = 0;	//optional int64  踢人者

		public string role_name = "";	//optional string

		public long[] kick_roles = null;	//repeated int64 踢掉的人

		public p_corps_member_info[] members = null;	//repeated p_corps_member_info

		public override int __ID(){ return 1312; }
	}

	public class m_corps_change_job_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//required bool

		public long role_id = 0;	//optional int64

		public string role_name = "";	//optional string

		public Int32 type = 0;	//optional int32	1指定为副队长，2取消副队长，3转移会长

		public override int __ID(){ return 1313; }
	}

	//知名度通知
	public class m_corps_popularity_notice_toc : BaseToSC
	{
		public Int32 add_popu = 0;	//required int32

		public override int __ID(){ return 1320; }
	}

	//贡献通知
	public class m_corps_fc_notice_toc : BaseToSC
	{
		public Int32 add_fc = 0;	//required int32

		public override int __ID(){ return 1321; }
	}

	public class m_corps_shop_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_corps_shop_goods[] all_goods = null;	//repeated p_corps_shop_goods  所有商品的信息

		public override int __ID(){ return 1316; }
	}

	public class m_corps_shop_buy_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type_id = 0;	//optional int32  商品ID

		public Int32 num = 1;	//optional int32  商品的数量

		public Int32 family_con = -1;	//optional int32 新的帮贡

		public p_goods[] goods_list = null;	//repeated p_goods

		public p_chip_info[] chip_list = null;	//repeated    p_chip_info 更新芯片背包

		public override int __ID(){ return 1317; }
	}

	//战队公告
	public class m_corps_notice_toc : BaseToSC
	{
		public p_msg notice = null;	//optional p_msg

		public override int __ID(){ return 1315; }
	}

	public class m_corps_get_chatinfo_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_user_chat_info[] chat_list = null;	//repeated p_user_chat_info

		public override int __ID(){ return 1318; }
	}

	public class m_corps_get_sys_hongbao_toc : BaseToSC
	{
		public Int32 type = 1;	//required int32  1是查询，2是新增

		public p_corps_sys_hongbao[] hongbao = null;	//repeated p_corps_sys_hongbao

		public override int __ID(){ return 1325; }
	}

	//红包通知
	public class m_corps_hongbao_notice_toc : BaseToSC
	{
		public p_corps_hongbao_info hongbao = null;	//optional p_corps_hongbao_info

		public override int __ID(){ return 1322; }
	}

	public class m_corps_hongbao_send_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_corps_hongbao_info hongbao_info = null;	//optional p_corps_hongbao_info

		public override int __ID(){ return 1323; }
	}

	public class m_corps_hongbao_get_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 0;	//optional int32 1次数已超， 2被领完 3已经领过 4 过期

		public p_corps_hongbao_info hongbao_info = null;	//optional p_corps_hongbao_info

		public override int __ID(){ return 1324; }
	}

	public class m_corps_hongbao_qry_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 hb_times = 0;	//optional int32 今天领红包次数

		public p_corps_hongbao_info[] hongbao_info = null;	//repeated p_corps_hongbao_info

		public override int __ID(){ return 1327; }
	}

	public class m_corps_qry_state_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_friend_info[] info_list = null;	//repeated p_friend_info 好友列表

		public override int __ID(){ return 1329; }
	}

	public class m_corps_hongbao_get_notice_toc : BaseToSC
	{
		public long role_id = 0;	//optional int64  踢人者

		public string role_name = "";	//optional string

		public override int __ID(){ return 1326; }
	}

	public class m_chat_user_msg_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_user_chat_info chat_info = null;	//optional p_user_chat_info

		public override int __ID(){ return 1102; }
	}

	public class m_chat_voice_query_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public long voice_id = 0;	//optional int64

		public byte[] voice_data = null;	//repeated byte    语音流

		public override int __ID(){ return 1108; }
	}

	public class m_chat_online_notice_toc : BaseToSC
	{
		public p_user_chat_info[] msg_info = null;	//repeated p_user_chat_info

		public override int __ID(){ return 1104; }
	}

	public class m_chat_sys_msg_toc : BaseToSC
	{
		public p_sys_chat_info msg_info = null;	//required p_sys_chat_info

		public override int __ID(){ return 1103; }
	}

	public class m_chat_team_msg_toc : BaseToSC
	{
		public long role_id = 0;	//optional int64

		public string role_name = "";	//optional string

		public Int32 type = 0;	//optional  int32 前端用于区分场景

		public string text = "";	//optional string 文字消息

		public override int __ID(){ return 1107; }
	}

	public class m_chat_loud_speaker_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] update_goods = null;	//repeated p_goods	更新背包

		public override int __ID(){ return 1109; }
	}

	public class m_chat_subscription_state_toc : BaseToSC
	{
		public long[] list = null;	//repeated int64		默认不在线,回了就是在线的

		public override int __ID(){ return 1110; }
	}

	public class m_chat_subscription_notice_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32		1上线,2下线

		public long[] role_id = null;	//repeated int64

		public override int __ID(){ return 1111; }
	}

	public class m_chat_unread_msg_toc : BaseToSC
	{
		public p_chat_msg_box[] msgs = null;	//repeated p_chat_msg_box

		public override int __ID(){ return 1106; }
	}

	public class m_chat_broadcast_toc : BaseToSC
	{
		public p_msg msg = null;	//required p_msg

		public override int __ID(){ return 1112; }
	}

	public class m_achieve_update_toc : BaseToSC
	{
		public Int32[] finish_list = null;	//repeated	int32

		public p_role_achieve[] ach_list = null;	//repeated	p_role_achieve	 进度变化的成就

		public override int __ID(){ return 1203; }
	}

	public class m_achieve_query_all_toc : BaseToSC
	{
		public Int32[] finish_list = null;	//repeated int32 已经完成的成就

		public p_role_achieve[] doing_list = null;	//repeated p_role_achieve 正在进行的成就 不含没有进度的

		public Int32[] award_list = null;	//repeated int32  已经领奖的成就点

		public override int __ID(){ return 1202; }
	}

	// 成就点数按照前端自己统计
	public class m_achieve_take_award_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional 	p_msg	操作失败原因

		public p_award[] award_list = null;	//repeated   p_award	获得列表

		public Int32 num = 0;	//optional	int32 	领取的成就个数

		public override int __ID(){ return 1204; }
	}

	//-----------------------------------------------center--------------------------------------
	public class m_center_connect_toc : BaseToSC
	{
		public string token = "";	//required string

		public string host = "";	//required string

		public Int32 port = 0;	//required int32

		public Int32 udp_port = 0;	//optional int32

		public string bgp_host = "";	//optional string

		public Int32 bgp_port = 0;	//optional int32

		public Int32 bgp_udp_port = 0;	//optional int32

		public long role_id = 0;	//required int64

		public override int __ID(){ return 1502; }
	}

	//战斗相关接口
	//发送数据给客户端，等待客户端加载
	public class m_fight_match_start_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 stageid = 0;	//optional   int32     关卡ID

		public p_player[] players = null;	//repeated   p_player    玩家属性

		public Int32 seed = 0;	//optional   int32   随机种子

		public p_player[] machine = null;	//repeated	p_player	机器人

		public override int __ID(){ return 1402; }
	}

	public class m_fight_match_finish_reload_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public long role_id = 0;	//optional   int64   玩家ID

		public Int32 progress = 0;	//optional	int32  进度

		public string version = "";	//optional string	版本

		public override int __ID(){ return 1406; }
	}

	//所有客户端加载完毕以后，正式开始多人战斗
	public class m_fight_match_begin_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public override int __ID(){ return 1407; }
	}

	public class m_fight_match_exit_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 type = 0;	//optional   int32   退出类型

		public long roleid = 0;	//optional   int64   用户放弃ID

		public Boolean ifreturn = false;	//optional   bool    false:离开,true:返回

		public override int __ID(){ return 1403; }
	}

	public class m_fight_match_result_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 victory = 0;	//optional   int32     获胜的阵营

		public override int __ID(){ return 1404; }
	}

	public class m_fight_match_end_sync_toc : BaseToSC
	{
		public Boolean sync = false;	//optional	bool	false表示不同步

		public override int __ID(){ return 1420; }
	}

	public class m_fight_match_recove_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 stage_id = 0;	//optional	int32	关卡id

		public Int32 seed = 0;	//optional	int32	随机种子

		public string players = "";	//optional   string  玩家

		public string machine = "";	//optional	string  机器人

		public string miss_packet = "";	//optional   string  战斗(帧)

		public override int __ID(){ return 1405; }
	}

	//战斗进程不存在的时候却收到了客户端的战斗消息协议，统一返回该消息以表示错误
	public class m_fight_match_error_toc
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

	}

	//战斗开始统一接口
	public class m_fight_start_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public Int32 fight_type = 0;	//optional	int32	关卡类型

		public Int32 ctrl_index = 0;	//optional	int32	玩家索引

		public Int32 stageid = 0;	//optional   int32 	关卡ID

		public Int32 seed = 0;	//optional   int32	随机种子

		public string players = "";	//optional   string	玩家 或 机器人

		public p_role_skill[] skills = null;	//repeated 	p_role_skill

		public string miss_packet = "";	//optional   string  操作(帧)

		public p_kvi[] para = null;	//repeated 	p_kvi	特殊数据

		public Int32 net_status = 0;	//optional   int32   网络战斗的状态，0是通常，1是战斗加载过程

		public Int32 net_type = 0;	//optional   int32   战斗的网络类型，0是单机，1是TCP，2是UDP，3是WIFI对战

		public Boolean is_reload = false;	//optional   bool 	是否重连

		public p_player[] pplayers = null;	//repeated   p_player	玩家 重连的时候设值

		public Int32 m_frame = 0;	//optional   int32  本包的最大帧

		public Int32 max_frame = 0;	//optional   int32

		public string version = "";	//optional string	战斗版本

		public string chat_name = "";	//optional string

		public override int __ID(){ return 1408; }
	}

	public class m_fight_finish_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public p_msg reason = null;	//optional	p_msg	操作失败原因

		public override int __ID(){ return 1409; }
	}

	public class m_fight_exit_fight_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean return_self = true;	//optional bool 			是否自己主动退出

		public override int __ID(){ return 1410; }
	}

	public class m_fight_change_tcp_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 type = 2;	//optional int32  1 udp切tcp 2 tcp 切UDP

		public Boolean is_reload = false;	//optional  bool

		public string miss_packet = "";	//optional   string  战斗(帧)

		public Int32 m_frame = 0;	//optional   int32  本包的最大帧

		public Int32 max_frame = 0;	//optional   int32

		public override int __ID(){ return 1411; }
	}

	public class m_fight_kill_info_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public override int __ID(){ return 1416; }
	}

	public class m_fight_skill_levelup_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 skill_id = 0;	//optional int32

		public Int32 ex_skill_id = 0;	//optional int32

		public Int32 level = 0;	//optional int32

		public override int __ID(){ return 1415; }
	}

	public class m_fight_udp_pack_toc : BaseToSC
	{
		public byte[] data = null;	//repeated byte

		public override int __ID(){ return 1412; }
	}

	public class m_fight_fail_apply_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long role_id = 0;	//optional int64

		public Int32 num = 0;	//optional int32

		public Int32 cool_time = 0;	//optional int32

		public override int __ID(){ return 1417; }
	}

	public class m_fight_fail_result_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Boolean is_self = true;	//optional bool

		public long[] role_ids = null;	//repeated int64 已经同意的人

		public Int32 num = 0;	//optional int32

		public Int32 type = 0;	//required int32 1投降 2拒绝投降 0还没出结果

		public override int __ID(){ return 1418; }
	}

	public class m_fight_time_check_toc : BaseToSC
	{
		public Boolean succ = true;	//required	bool	操作成功

		public override int __ID(){ return 1414; }
	}

	public class m_stage_time_check_toc
	{
		public Boolean succ = true;	//required	bool	操作成功

	}

	public class m_common_notice_toc : BaseToSC
	{
		public p_msg msg = null;	//optional p_msg

		public override int __ID(){ return 1604; }
	}

	public class m_common_msg_box_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32

		public p_msg msg = null;	//optional p_msg

		public override int __ID(){ return 1605; }
	}

	public class m_common_lottery_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_client_good[] goods_list = null;	//repeated p_client_good 抽取的物品列表

		public p_client_good award_goods = null;	//optional p_client_good 抽中的物品

		public override int __ID(){ return 1602; }
	}

	public class m_common_client_update_toc : BaseToSC
	{
		public string version = "";	//optional string	版本

		public override int __ID(){ return 1603; }
	}

	public class m_common_active_start_toc : BaseToSC
	{
		public p_kvi[] list = null;	//repeated p_kvi

		public override int __ID(){ return 1607; }
	}

	public class m_common_active_end_toc : BaseToSC
	{
		public Int32 id = 0;	//required int32

		public override int __ID(){ return 1608; }
	}

	public class m_login_award_get_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 award_type = 0;	//optional int32  		1.七日奖励

		public Int32 days = 0;	//optional int32  		领第几天的,直接返回的tos里的值

		public p_award[] update_goods = null;	//repeated p_award		更新背包的东西

		public override int __ID(){ return 1703; }
	}

	//上线时候推送的七日奖励信息
	public class m_login_award_info_toc : BaseToSC
	{
		public p_awards_info[] list = null;	//repeated p_awards_info

		public override int __ID(){ return 1702; }
	}

	//变化的时候推送的奖励信息
	public class m_login_award_notice_toc : BaseToSC
	{
		public p_awards_info[] list = null;	//repeated p_awards_info

		public override int __ID(){ return 1704; }
	}

	//通用活动开始结束弹出提示接口
	public class m_festival_notice_toc
	{
		public Int32 id = 0;	//required int32		表示什么活动：1 领取体力 2充值以后，通知领首充

		public Int32 type = 1;	//required int32		1表示开始，0表示结束

		public Int32 reset_time = 0;	//optional int32		倒计时时间，根据不同的活动而定 1领体力时，该字段为1表示可以领取

	}

	public class m_festival_card_get_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_festival_card_info[] card_list = null;	//repeated p_festival_card_info

		public override int __ID(){ return 1818; }
	}

	public class m_festival_card_exchange_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] update_goods = null;	//repeated p_goods

		public override int __ID(){ return 1808; }
	}

	public class m_festival_fcode_get_info_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32   0:未领取  1：已领取   2：过期

		public override int __ID(){ return 1810; }
	}

	public class m_festival_fcode_exchange_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] update_goods = null;	//repeated p_goods

		public override int __ID(){ return 1809; }
	}

	public class m_festival_get_info_toc : BaseToSC
	{
		public p_festival_info[] list = null;	//repeated p_festival_info

		public p_festival_info[] prep_list = null;	//repeated p_festival_info

		public override int __ID(){ return 1802; }
	}

	public class m_festival_get_award_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 id = 0;	//optional int32

		public Int32 key = 0;	//optional int32

		public p_goods[] update_goods = null;	//repeated p_goods

		public Int32 get_times = -1;	//optional int32

		public override int __ID(){ return 1803; }
	}

	public class m_festival_notic_id_toc : BaseToSC
	{
		public p_kvi[] list = null;	//repeated p_kvi

		public override int __ID(){ return 1804; }
	}

	public class m_festival_notic_del_toc : BaseToSC
	{
		public Int32 type = 0;	//required int32

		public Int32 id = 0;	//required int32

		public Int32 value = 0;	//optional int32

		public override int __ID(){ return 1805; }
	}

	public class m_festival_notice_start_toc : BaseToSC
	{
		public Int32[] ids_list = null;	//repeated int32

		public override int __ID(){ return 1807; }
	}

	public class m_festival_notice_award_toc : BaseToSC
	{
		public Boolean type = true;	//required bool  true:进度更新   false：活动结束

		public Int32 id = 0;	//required int32

		public p_award_btn btn_info = null;	//optional p_award_btn

		public override int __ID(){ return 1806; }
	}

	public class m_festival_slot_get_toc : BaseToSC
	{
		public Int32 times = 0;	//required int32

		public Int32 last_award = 0;	//required int32 上一次的奖励ID

		public p_slot_log[] logs = null;	//repeated p_slot_log

		public override int __ID(){ return 1811; }
	}

	public class m_festival_slot_start_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 times = 0;	//optional int32

		public Int32 last_award = 0;	//optional int32

		public override int __ID(){ return 1812; }
	}

	public class m_festival_slot_get_award_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 id = 0;	//optional int32

		public Int32 key = 0;	//optional int32

		public p_goods[] update_goods = null;	//repeated p_goods

		public override int __ID(){ return 1813; }
	}

	public class m_festival_slot_use_item_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public p_goods[] update_goods = null;	//repeated p_goods

		public Int32 last_award = 0;	//optional int32 奖励ID

		public override int __ID(){ return 1814; }
	}

	public class m_festival_lucky_bag_toc : BaseToSC
	{
		public Boolean succ = false;	//required bool

		public p_msg reason = null;	//optional p_msg

		public Int32 id = 0;	//optional int32	活动id

		public Int32 type = 0;	//optional int32	类型

		public Int32 times = 0;	//optional int32	剩余免费次数

		public p_goods[] update_goods = null;	//repeated p_goods 更新背包

		public p_lucky_bag_history[] history_rec = null;	//repeated p_lucky_bag_history 历史记录

		public override int __ID(){ return 1815; }
	}

}

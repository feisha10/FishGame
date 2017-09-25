using System;
using System.Collections.Generic;

namespace Client
{
	//通用int型key，value对
	public class p_kvi
	{
		public Int32 key = 0;	//required	int32

		public Int32 value = 0;	//required	int32

	}

	public class p_kvf
	{
		public Int32 key = 0;	//required	int32

		public float value = 0.0f;	//required	float

	}

	public class p_kvl
	{
		public Int32 key = 0;	//required	int32

		public long value = 0;	//required	int64

	}

	public class p_dial
	{
		public Int32 index = 0;	//required	int32

		public Int32 good_id = 0;	//required	int32

		public Int32 num = 0;	//required	int32

		public Boolean flag = false;	//required	bool

	}

	public class p_kvs
	{
		public Int32 key = 0;	//required	int32

		public string value = "";	//required	string

	}

	//通用int64型key(64可放角色id)，value对
	public class p_rvi
	{
		public long key = 0;	//required	int64

		public Int32 value = 0;	//required	int32

	}

	//通用型三字段结构
	public class p_event
	{
		public Int32 id = 0;	//required	int32

		public Int32 times = 0;	//required	int32

		public Int32 last_time = 0;	//required	int32

	}

	// {角色id，时间，类型}
	public class p_rvvi
	{
		public long id = 0;	//required	int64

		public Int32 time = 0;	//required	int32

		public Int32 type = 0;	//required	int32

	}

	public class p_fight_result
	{
		public long role_id = 0;	//required	int64	角色id

		public string role_name = "";	//optional	string

		public Int32 hero_type = 0;	//optional	int32

		public Int32 kill_num = 0;	//optional	int32

		public Int32 dead_num = 0;	//optional   int32

		public Int32 assists_num = 0;	//optional	int32

		public Boolean is_mvp = false;	//optional   bool

		public Int32 dashen_num = 0;	//optional   int32   大神次数

		public Int32 chaoshen_num = 0;	//optional   int32   超神次数

		public Int32 kill3_num = 0;	//optional   int32 	三杀次数

		public Int32 kill4_num = 0;	//optional   int32 	四杀次数

		public Int32 kill5_num = 0;	//optional   int32 	五杀次数

		public Boolean is_ai = false;	//optional   bool 	是否挂机

		public Int32 kill_tower = 0;	//optional int32		推塔数

		public Int32[] equips = null;	//repeated	int32

		public Int32 score = 0;	//required	int32 评分

		public Int32 rank = 5;	//required	int32 本方排名

		public Int32 damage = 0;	//required	int32 伤害

		public Int32 camp = 1;	//required	int32

		public Int32 gold = 0;	//required	int32

		public Int32 def_damage = 0;	//required	int32 承受伤害

	}

	//角色图标信息
	public class p_role_view_icon
	{
		public Int32 id = 0;	//required	int32

		public string name = "";	//required	string

		public Int32 head = 0;	//required	int32

	}

	//角色技能信息
	public class p_role_skill
	{
		public Int32 skill_id = 0;	//required	int32

		public Int32 ex_skill_id = 0;	//required	int32

		public Int32 level = 0;	//required	int32

	}

	//角色基础属性，极度稀罕会改变的数据放这里
	public class p_role_base
	{
		public long role_id = 0;	//required	int64	角色id

		public string role_name = "";	//required	string	角色名

		public string head_addr = "";	//optional	string	头像路径

		public string sign_str = "";	//optional	string	个性签名

		public string account_name = "";	//required	string	帐号

		public Int32 level = 0;	//required   int32

		public Int32 exp = 0;	//required   int32	经验

		public Int32 double_exp_time = 0;	//required	int32	双倍经验时间

		public Int32 create_time = 0;	//required	int32	创建时间

		public Int32 server_id = 0;	//required	int32	初始建号的原服务器id

		public Int32 agent_id = 0;	//required	int32	初始建号的原代理商id

		public string last_login_ip = "";	//required	string	最后登录IP

		public Int32 last_login_time = 0;	//required	int32	最后登录时间

		public Int32 last_offline_time = 0;	//required   int32   上次离线时间

		public long family_id = 0;	//required	int64   公会id

		public string family_name = "";	//required	string   公会名称

		public Int32 family_leave_time = 0;	//required	int32   上次退出公会时间

		public Int32 family_office = 0;	//optional	int32   公会官职

		public Int32 fight_power = 0;	//required	int32	%%用于战队签到

		public Int32 login_days = 1;	//required	int32	登陆天数

		public Int32 online_time = 0;	//required   int32	总共在线时长

		public p_kvi online_today_time = null;	//required   p_kvi	今日在线时长:key日期,value时长

		public string[] used_names = null;	//repeated	string	曾用名列表

		public Int32 device_type = 1;	//optional	int32	1安卓 2ios 3 web 4 pc

		public long clan_id = 0;	//optional	int64	家族id

		public Int32 lang_id = 0;	//optional	int32	语言包

		public Int32 dan_grading = 1;	//optional   int32   段位

		public Int32 high_grading = 1;	//optional   int32   历史最高段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 allow_qry = 0;	//optional	int32	用来表示是否允许别人查看， 0表示允许， 1表示不允许

		public Int32 double_exp_typeid = 0;	//optional	int32	双倍经验的道具ID

		public Int32 double_exp_times = 0;	//optional	int32	双倍经验次数

		public Int32 login_award = 0;	//optional	int32	回归奖励 超过5日登陆非0表示可以领奖励

		public Int32 week_award_time = 0;	//optional	int32	周一惊喜领奖时间

		public string head_addr_tmp = "";	//optional	string	待审核头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32[] head_frame_list = null;	//repeated	int32	拥有的头像框

	}

	//角色属性
	public class p_role_attr
	{
		public long role_id = 0;	//required	int64

		public Int32 gold = 0;	//required	int32	点券

		public Int32 bind_gold = 0;	//required	int32	合金

		public Int32 silver = 0;	//required	int32	金币

		public Int32 last_time = 0;	//required	int32	上次获得金币时间

		public Int32 week_silver = 0;	//required	int32   本周获得金币数

		public Int32 double_time = 0;	//required	int32	双倍金币时间

		public Int32 bind_silver = 0;	//required	int32	银两(绑定)

		public Int32 sum_pay_gold = 0;	//required	int32	总充值钻石

		public Int32 prestige = 0;	//required	int32	声望

		public Int32 current_vit = 0;	//required	int32	当前的体力值

		public Int32 last_recover_time = 0;	//required	int32	上次恢复的时间点

		public Int32 vip_grade = 0;	//required	int32	vip等级

		public Int32 vip_point = 0;	//required	int32	vip点数

		public Int32 double_typeid = 0;	//optional	int32	双倍金币的道具ID

		public Int32 sweet = 0;	//optional	int32	糖果--推广积分

		public Int32 double_times = 0;	//optional	int32	双倍金币的次数

		public Int32 wafer_chip = 0;	//optional	int32	购买芯片的晶片

		public Int32 contribution = 0;	//required int32   	贡献

		public Int32 today_fc = 0;	//required int32		今日贡献

		public Int32 last_fc_time = 0;	//required int32 	上次获得贡献时间

	}

	//皮肤
	public class p_skin
	{
		public Int32 skin_id = 0;	//optional   int32   皮肤ID

		public Int32 valid_time = 0;	//optional   int32   有效时间 大于0为有效时间戳，小于当前时间为为失效 -1是永久

	}

	public class p_equip_info
	{
		public Int32 plan_id = 0;	//required   int32   装备方案ID

		public string plan_name = "";	//optional   string   装备方案名称

		public Int32[] equip_list = null;	//repeated 	int32	方案

	}

	//英雄
	public class p_hero
	{
		public Int32 hero_type = 0;	//required   int32	类型id

		public Int32 valid_time = 0;	//required   int32   有效时间 大于0为有效时间戳，小于当前时间为为失效 -1是永久

		public Int32 skin_id = 0;	//optional   int32   穿戴皮肤 0表示没有

		public p_skin[] skin_list = null;	//repeated 	p_skin	拥有的皮肤

		public Int32 equip_id = 0;	//optional   int32   使用的装备方案

		public p_equip_info[] equip_list = null;	//repeated 	p_equip_info	装备方案

		public Int32 use_exp = 0;	//optional   int32  熟练度

	}

	//奖励定义
	public class p_award
	{
		public Int32 type = 0;	//required	int32	奖励类型，如上  %0、无（一般用于补全概率权重） 1、经验；2、钻石；3、金币  4、合金  5、物品(道具,装备,防具,符石,时装) 6 晶片 7 友情点 10、完整的物品（p_goods结构） 12:皮肤  17:英雄 其他以后需要再添加，然后在player_misc添加相应的函数

		public Int32 param = 0;	//optional	int32	参数列表   类型为5时，参数为TypeID；类型1时，参数为是否加储备经验(0否1是)

		public Int32 count = 1;	//optional	int32	数量

		public Boolean bind = false;	//optional	bool	是否绑定

		public Int32 weight = 0;	//optional	int32	权重（0、必给；>0、所有奖励加权算随机；<0、独立万分比概率。）

		public p_goods goods = null;	//optional	p_goods 完整的p_goods结构, type为10专用

	}

	//符石镶嵌在装备上的信息
	public class p_stone
	{
		public Int32 pos = 0;	//required    int32   孔的位置 1,2,3,4,5

		public Int32 type_id = 0;	//required    int32   物品ID

		public Int32 sub_type = 0;	//required    int32   符石类型

		public Boolean bind = false;	//required    bool	是否绑定

	}

	public class p_equip
	{
		public Int32 current_colour = 0;	//required	int32	颜色  				神器颜色

		public Int32 loadposition = 0;	//required   int32	部位编号

		public Int32 grade = 0;	//required   int32	装备强化等级		强化等级

		public Int32 exp = 0;	//optional	int32			 		属性强化等级

		public p_kvi[] property = null;	//repeated 	p_kvi   基础属性			基础属性

		public p_kvi[] add_property = null;	//repeated   p_kvi   附加属性 			精炼属性

		public p_stone[] stones = null;	//repeated   p_stone	附加属性

		public Int32 succ_rate = 0;	//optional	int32	淬炼的概率			极品度

		public Int32 bless_level = 0;	//optional	int32	 				 战力评分

		public Int32 bless_prop_level = 0;	//optional	int32

		public Int32 type = 0;	//optional	int32	装备类型	0:普通  1：普通打造  2：强化打造

		public p_kvi[] add_effects = null;	//repeated	p_kvi	附加效果

		public string create_name = "";	//optional	string	打造者名字

		public p_kvi[] bless_list = null;	//repeated	p_kvi	神佑完美度

		public Int32 seal_used_times = 0;	//required	int32	封印使用了的次数

		public Int32 unused = 0;	//required	int32	未使用的字段

		public Int32 score = 0;	//required	int32	装备属性评分(原先记录解冻时间，该时间值已放到p_goods中)

	}

	public class p_goods
	{
		public Int32 id = 0;	//required    int32   物品唯一标示ID

		public Int32 type_id = 0;	//required    int32   物品ID

		public Int32 type = 0;	//required    int32   物品类型

		public Boolean bind = false;	//required    bool	是否绑定

		public Int32 current_num = 1;	//required    int32   数量

		public Int32 start_time = 0;	//required    int32	开始时间

		public Int32 end_time = 0;	//required    int32	过期时间

		public p_equip ext_info = null;	//optional    p_equip

		public Int32 lock_time = 0;	//optional 	int32	解冻时间

		public Int32 create_time = 0;	//required	int32	创建时间

	}

	public class p_prop
	{
		public Int32 power = 0;	//required    int32	力量

		public Int32 quick = 0;	//required    int32	敏捷

		public Int32 con = 0;	//required    int32	体质

		public Int32 brains = 0;	//required    int32	智力

		public Int32 max_hp = 0;	//required    int32	最大生命

		public Int32 phy_attack = 0;	//required    int32	物理攻击

		public Int32 magic_attack = 0;	//required    int32	魔法攻击

		public Int32 phy_defence = 0;	//required    int32	物理防御

		public Int32 magic_defence = 0;	//required    int32	魔法防御

		public Int32 hit = 0;	//required    int32	命中

		public Int32 miss = 0;	//required    int32	闪避

		public Int32 crit = 0;	//required    int32	暴击

		public Int32 tenacity = 0;	//required    int32	韧性

	}

	public class p_broadcast
	{
		public Int32 type = 0;	//required int32	类型 1登入公告 2活动公告

		public Int32 time = 0;	//required int32 时间

		public string title = "";	//required string 标题

		public string content = "";	//required string 内容

	}

	public class p_login_notice
	{
		public Int32 id = 0;	//required int32 id

		public Int32 type = 0;	//required int32	类型 1登入公告 2活动公告

		public Int32 time = 0;	//required int32 时间

		public string title = "";	//required string 标题

		public string content = "";	//required string 内容

		public string event_str = "";	//optional string 事件

	}

	public class p_msg
	{
		public Int32 id = 0;	//required int32	id

		public string msg = "";	//optional string

	}

	public class p_task
	{
		public Int32 id = 0;	//required int32	任务id

		public Int32 progress = 0;	//required int32	任务进度

		public Int32 accept_time = 0;	//required int32	任务接受时间

	}

	public class p_task_finish
	{
		public Int32 id = 0;	//required int32	任务id

		public Int32 times = 0;	//required int32	次数

		public Int32 finish_time = 0;	//required int32	任务完成时间

	}

	//玩家称号详细信息
	public class p_title
	{
		public long title_id = 0;	//required int64

		public string title_name = "";	//required string

		public Boolean auto_timeout = false;	//required bool 	是否自动过期

		public Int32 timeout_time = 0;	//optional int32	自动过期时间

	}

	public class p_ranking_role_level
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public Int32 level = 0;	//required	int32 	等级

		public string family_name = "";	//required	string  公会名字

		public Int32 exp = 0;	//required	int32	储备经验

	}

	public class p_account_role
	{
		public long role_id = 0;	//required int64 角色ID

		public string role_name = "";	//required string 角色名

		public Int32 level = 0;	//required int32 等级

		public Int32 fashion = 0;	//optional int32	时装衣服

	}

	// =================商店==========================
	//商店的信息
	public class p_shop_info
	{
		public Int32 id = 0;	//required int32 	商店id

		public string name = "";	//required string    商店的名字

		public p_shop_goods_info[] all_goods = null;	//repeated p_shop_goods_info  该商店所有商品的信息

	}

	public class p_shop_goods_info
	{
		public Int32 type_id = 0;	//required int32 物品ID

		public Int32 type = 0;	//required int32 类型

		public Int32 price_type = 0;	//required int32 1全部满足 2任意一种 5兑换

		public p_kvi[] prices = null;	//repeated p_kvi p_kvi,key=(Money类型, 1钻 2绑钻 3金币 4绑定金币 5 兑换此时typeid); value=价格(price_type=5时,为需要的数量)

		public Int32 cost_price_type = 0;	//required int32 原价类型

		public p_kvi[] cost_prices = null;	//repeated p_kvi 原价

		public string rebate = "";	//optional string 折扣说明

		public Int32 level_limit = 0;	//required int32 最低等级要求   配成负值,可看不可买

		public Boolean buy_batch = false;	//required bool  true可以批量购买

		public Boolean bind = false;	//required bool  购买后是否绑定

		public Int32 class_type = 0;	//required int32 分类

		public Int32 limit_type = 0;	//required int32 1:不限量，2：每日，3：总共

		public Int32 limit_num = 0;	//required int32 限购数量，根据limit_type判断显示

		public Int32 num = 0;	//required int32 剩余数量，根据limit_type判断显示

		public Boolean is_new = false;	//required bool 是否新品上架

		public Int32 arg = 0;	//required int32

	}

	public class p_shop_buy_info
	{
		public Int32 type_id = 0;	//required	int32 	ID

		public Int32 type = 0;	//required	int32 	类别

		public Int32 num = 0;	//optional	int32  	购买个数

	}

	// ===================== 邮件系统============================
	public class p_letter
	{
		public Int32 seq = 0;	//required int32 	邮箱内邮件编号，

		public long sender_id = 0;	//required int64  	发信人id， 系统邮件为0  -1是系统送花

		public p_letter_msg sender_name = null;	//optional p_letter_msg  	发信人名字

		public long recv_id = 0;	//required int64  	收信人id

		public p_letter_msg title = null;	//optional p_letter_msg  	标题

		public Int32 has_read = 1;	//optional int32	 	1未读 2已读 3已领取

		public p_letter_msg text = null;	//optional p_letter_msg  	正文

		public p_award[] attachment = null;	//repeated p_award   附件物品

		public Int32 recv_time = 0;	//optional int32  	接收时间

	}

	public class p_client_good
	{
		public Int32 type_id = 0;	//required int32	物品id  1源石 3金币 4 合金， 6英雄，7 皮肤，  其他：道具type_id

		public Int32 num = 0;	//required int32  物品数量

	}

	public class p_ranking_role_info
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//optional	int64 	角色ID

		public string role_name = "";	//optional	string  角色名

		public string head_addr = "";	//optional   string  头像路径

		public Int32 level = 0;	//optional	int32 	等级

		public Int32 value = 0;	//optional	int32	排行key

		public Int32 value2 = 0;	//optional   int32

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

	}

	//--------------------------------------好友  Start--------------------------------------
	public class p_friend_info
	{
		public long role_id = 0;	//required   int64   角色ID

		public string role_name = "";	//required   string  角色名称

		public string head_addr = "";	//optional   string  头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public string sign_str = "";	//optional	string	个性签名

		public Int32 sex = 0;	//optional   int32   男女

		public Int32 role_level = 0;	//optional   int32   等级

		public Int32 dan_grading = 0;	//optional   int32   段位

		public Int32 high_grading = 0;	//optional   int32   历史最高段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 state = 0;	//optional   int32   状态

		public Int32 state_time = 0;	//optional   int32   状态时间

		public Int32 friendly = 0;	//optional	int32	好友度

		public Int32 vip_grade = 0;	//optional	int32	vip等级

		public Int32 hero_num = 0;	//optional	int32	好友度

	}

	public class p_role_friend_request
	{
		public long role_id = 0;	//required   int64   角色ID

		public string role_name = "";	//required   string  角色名称

		public string head_addr = "";	//optional   string  头像路径

		public Int32 sex = 0;	//optional   int32   男女

		public Int32 role_level = 0;	//optional   int32   等级

		public Int32 dan_grading = 0;	//optional   int32   段位

		public Int32 high_grading = 0;	//optional   int32   历史最高段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public string request_msg = "";	//optional	string	验证信息

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 vip_grade = 0;	//required	int32	vip等级

		public Int32 hero_num = 0;	//optional	int32	好友度

		public Int32 skin_num = 0;	//optional	int32  	皮肤数量

		public Int32 skin_rarity = 0;	//optional	int32  	皮肤稀有度

	}

	public class p_friend_rank_info
	{
		public long role_id = 0;	//required   int64   角色ID

		public string role_name = "";	//required   string  角色名称

		public string head_addr = "";	//optional   string  头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 role_level = 0;	//optional   int32   等级

		public Int32 dan_grading = 0;	//optional   int32   段位

		public Int32 star = 0;	//optional   int32   段位

		public Int32 win_times = 0;	//optional   int32   胜场

		public Int32 mvp_times = 0;	//optional	int32  	全场最佳

		public Int32 skin_num = 0;	//optional	int32  	皮肤数量

		public Int32 skin_rarity = 0;	//optional	int32  	皮肤稀有度

	}

	//--------------------------------------好友  End----------------------------------------
	//
	public class p_near_info
	{
		public long role_id = 0;	//required   int64   角色ID

		public string role_name = "";	//required   string  角色名称

		public string head_addr = "";	//optional   string  头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 role_level = 0;	//optional   int32   等级

		public Int32 dan_grading = 0;	//optional   int32   段位

		public Int32 high_grading = 0;	//optional   int32   历史最高段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 state = 0;	//optional   int32   状态

	}

	//
	//战队信息
	public class p_corps_info
	{
		public long corps_id = 0;	//required int64

		public string corps_name = "";	//required string

		public string corps_key = "";	//required string 公会唯一ID

		public Int32 level = 1;	//required int32 公会等级

		public Int32 popularity = 0;	//required int32 知名度

		public Int32 levelup_time = 0;	//required int32 升级时间

		public long create_role_id = 0;	//required int64 创建者

		public long owner_role_id = 0;	//required int64 会长

		public string owner_role_name = "";	//required string 会长

		public long[] second_owner_roleid = null;	//repeated int64 副会长

		public string notice = "";	//required string  公告

		public p_kvi[] join_condition = null;	//repeated p_kvi   加入条件 1有等级要求，2段位要求

		public Boolean auto_agree = false;	//optional bool 自动审批

		public p_corps_member_info[] members = null;	//repeated p_corps_member_info   族员

		public p_corps_hongbao_info[] hb_info = null;	//repeated p_corps_hongbao_info 红包

		public Int32 week_popu = 0;	//optional int32 本周知名度

	}

	//战队简略信息
	public class p_corps_summary
	{
		public long corps_id = 0;	//required int64

		public string corps_name = "";	//required string

		public string corps_key = "";	//required string 唯一ID

		public Int32 level = 0;	//required int32 公会等级

		public long owner_role_id = 0;	//required int64 会长

		public string owner_role_name = "";	//required string 会长

		public p_kvi[] join_condition = null;	//repeated p_kvi   加入条件 1有等级要求，2段位要求

		public Int32 member_num = 0;	//required int32  会员人数

		public string notice = "";	//optional string  公告

		public Int32 week_popu = 0;	//optional int32 本周知名度

	}

	//战队成员信息
	public class p_corps_member_info
	{
		public long role_id = 0;	//required int64

		public string role_name = "";	//required string

		public Int32 role_level = 0;	//required int32

		public string head_addr = "";	//optional string	头像路径

		public Int32 head_frame = 0;	//optional int32	使用的头像框

		public Int32 contribution = 0;	//required int32   	贡献

		public Int32 total_fc = 0;	//required int32 	总贡献

		public Int32 today_fc = 0;	//required int32		今日贡献

		public Int32 last_fc_time = 0;	//required int32 	上次获得贡献时间

		public Int32 today_popularity = 0;	//required int32		今日获得的知名度

		public Boolean online = false;	//required bool

		public Int32 office = 6;	//required int32	官职，队长1，副队长2， 普通6

		public Int32 last_login = 0;	//optional int32	上次下线时间戳

		public Int32 dan_grading = 1;	//optional int32   段位

		public Int32 king_time = 0;	//optional int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 vip_grade = 0;	//optional int32

		public Int32 hb_times = 0;	//optional int32  今天红包次数 废弃

		public Int32 hb_time = 0;	//optional int32  上次抢红包时间  废弃

	}

	//战队红包信息
	public class p_corps_hongbao_member
	{
		public long role_id = 0;	//required int64

		public string role_name = "";	//optional string

		public Int32 gold = 0;	//optional int32

		public Int32 time = 0;	//optional int32

	}

	//战队红包信息
	public class p_corps_hongbao_info
	{
		public Int32 id = 0;	//required int32

		public long role_id = 0;	//required int64

		public string role_name = "";	//optional string

		public string head_addr = "";	//optional string	头像路径

		public Int32 head_frame = 0;	//optional int32	使用的头像框

		public string hongbao_desc = "";	//optional string 红包描述

		public Int32 time = 0;	//optional int32 失效时间

		public Int32 gold = 0;	//optional int32 总源石数

		public Int32 cur_gold = 0;	//optional int32 剩余的源石

		public Int32 total_num = 0;	//optional int32 总共个数

		public Int32 get_num = 0;	//optional int32 领取次数

		public p_corps_hongbao_member[] list = null;	//repeated p_corps_hongbao_member

		public Int32 sys_id = 0;	//optional int32 系统红包ID，等于0就是源石红包

		public Int32 type = 1;	//optional int32 红包的货币类型，1是金币，2是源石

	}

	//系统触发红包信息
	public class p_corps_sys_hongbao
	{
		public Int32 id = 0;	//required int32

		public Int32 gold = 0;	//optional int32 总源石数

		public Int32 num = 0;	//optional int32 总个数

		public Int32 exp_time = 0;	//optional int32 失效时间

	}

	//公会申请信息
	public class p_corps_role_request
	{
		public long role_id = 0;	//required int64

		public string role_name = "";	//required string

		public Int32 role_level = 0;	//required int32

		public Int32 dan_grading = 1;	//optional   int32   段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 vip_grade = 0;	//optional	int32

		public string head_addr = "";	//optional	string	头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

	}

	//公会兑换商店商品信息
	public class p_corps_shop_goods
	{
		public Int32 type_id = 0;	//required int32 物品ID

		public Int32 type = 0;	//required int32 类型

		public string name = "";	//required string 名字

		public Int32 price = 0;	//required int32 贡献

		public Boolean bind = false;	//required bool  购买后是否绑定

	}

	//聊天	加字段记得要升级DB_ROLE_CHAT_MSG_BOX
	public class p_user_chat_info
	{
		public Int32 msg_type = 0;	//required int32		消息类型

		public long role_id = 0;	//required int64		角色ID

		public string role_name = "";	//optional string

		public long target_id = 0;	//optional int64	 	接口者ID

		public string target_name = "";	//optional string 	接收者名字

		public Boolean is_read = false;	//optional  bool 是否已读

		public string head_addr = "";	//optional string	头像路径

		public string text = "";	//optional string

		public long voice_id = 0;	//optional int64     语音ID,0表示非语音

		public Int32 voice_time = 0;	//optional int32

		public Int32 color_id = 0;	//optional int32 1：白色

		public Int32 msg_id = 0;	//optional int32	私聊消息id

		public Int32 datetime = 0;	//optional int32	时间

		public Int32 level = 0;	//optional int32 角色等级

		public Boolean is_has_family = false;	//optional bool	是否有公会

		public Int32 vip_grade = 0;	//optional int32	vip等级

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 family_office = 0;	//optional	int32	公会官职

	}

	//聊天	加字段记得要升级DB_ROLE_CHAT_MSG_BOX
	public class p_chat_msg_box
	{
		public long sender = 0;	//required int64		角色ID

		public string sender_name = "";	//optional string

		public string head_addr = "";	//optional string	头像路径

		public Int32 level = 0;	//optional int32 角色等级

		public p_user_chat_info[] msg = null;	//repeated p_user_chat_info

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

	}

	public class p_sys_chat_info
	{
		public Int32 msg_type = 0;	//required int32		消息类型

		public p_msg msg = null;	//optional p_msg

	}

	//--------------------------------------成就 achieve---------------------------------------
	public class p_role_achieve
	{
		public Int32 id = 0;	//required	int32

		public Int32 count = 0;	//optional	int32

	}

	//多人玩家信息
	public class p_player
	{
		public long role_id = 0;	//required   int64

		public string role_name = "";	//optional   string

		public Int32 hero_type = 0;	//required   int32   英雄模型ID

		public Int32 vip_grade = 0;	//optional   int32

		public Int32 state = 0;	//required  	int32 	1确认英雄，0未确认

		public p_kvi[] prop = null;	//repeated   p_kvi  英雄属性

		public Int32[] skill_on = null;	//repeated   int32   带上场的技能列表

		public Int32 skin_id = 0;	//required	int32	主角模型

		public Int32 camp = 0;	//required   int32   阵营ID

		public Int32 dan_grading = 0;	//optional	int32  	段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 ai_flag = 0;	//required 	int32	 托管标志

		public p_equip_info equip_info = null;	//optional   p_equip_info 装备方案

	}

	//-----------------------------------------------------------------------
	public class p_awards_info
	{
		public Int32 award_type = 0;	//required int32			1.七日奖励 4,循环七日登录

		public Int32 days = 0;	//required int32			登陆天数

		public p_kvi[] list = null;	//repeated p_kvi			{p_kvi,天数,状态(0,不可领,1可领,2已领)}

		public p_award_list[] all_awards = null;	//repeated p_award_list		全部奖励的信息

	}

	public class p_award_list
	{
		public p_award[] list = null;	//repeated p_award

	}

	public class p_festival_card_info
	{
		public Int32 batch_id = 0;	//required int32 批次号

		public Int32 agent_id = 0;	//required int32 所属平台ID

		public string desc = "";	//required string 描述

	}

	public class p_festival_info
	{
		public Int32 id = 0;	//required int32

		public Int32 start_time = 0;	//required int32

		public Int32 end_time = 0;	//required int32

		public p_award_btn[] award_btn = null;	//repeated p_award_btn

		public p_fes_info festival_info = null;	//optional p_fes_info

	}

	public class p_award_btn
	{
		public Int32 id = 0;	//required int32

		public Int32 cur_num = 0;	//optional int32

		public Int32 state = 0;	//required int32

	}

	public class p_fes_award
	{
		public Int32 type_id = 0;	//required int32	奖励配置id

		public Int32 num = 0;	//required int32 数量

		public Boolean bind = false;	//required bool	绑定

		public Boolean glow = false;	//required bool 流光

		public Int32 hero_type = 0;	//required int32 职业

	}

	public class p_fes_btn
	{
		public Int32 btn_id = 0;	//required int32		按钮id

		public Boolean show_btn = false;	//required bool		是否显示领奖按钮

		public p_fes_award[] award_list = null;	//repeated p_fes_award 奖励物品列表

		public string title = "";	//required string 	奖励title

		public string desc = "";	//required string	按钮描述

		public Int32 goal_num = 0;	//required int32		目标进度

		public Int32 check_type = 0;	//required int32		//检测领奖状态类型（0:后端判断是否可领;1:等级判断是否可领;2:vip等级判断是否可领4,前端根据curnum以及goalnum判断状态）

		public string param = "";	//required string	其他

	}

	public class p_fes_info
	{
		public Int32 id = 0;	//required int32	 	活动id

		public string name = "";	//required string	活动名字

		public string desc = "";	//required string	活动描述

		public string time_desc = "";	//required string	活动时间描述

		public Boolean prep_show = false;	//required bool		预告阶段是否显示 false:不显示  true:显示

		public Int32[] link_id = null;	//repeated int32		链接id

		public string[] link_text = null;	//repeated string	链接文本

		public Int32 view_type = 0;	//required int32		//显示类型(1普通领奖界面当普通领奖界面同是含有跳转功能是就是包涵跳转功能能的领奖界面;2普通跳转界面;3vip奖励界面;4竞技排行界面;5为摇奖界面;6签到;7显示进度的领奖界面;8单笔重置奖励;101高级转盘

		public Int32 level = 0;	//required int32		所需等级

		public Int32 sort_index = 0;	//required int32		排序

		public p_fes_btn[] btn_info = null;	//repeated p_fes_btn	按钮信息

		public string param = "";	//required string	其他信息

	}

	public class p_slot_log
	{
		public string role_name = "";	//optional string

		public Int32 typeid = 0;	//optional int32

		public Int32 num = 0;	//optional int32

	}

	public class p_rank_lucky_bag
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public long server_id = 0;	//required	int64	服务器id

		public Int32 bag_count = 0;	//required	int32	福袋个数

	}

	public class p_lucky_bag_history
	{
		public Int32 color = 0;	//required int32			福袋颜色

		public p_client_good client_good = null;	//required p_client_good 获得的物品

	}

	public class p_rank_pay
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public long server_id = 0;	//required	int64	服务器id

		public Int32 pay = 0;	//required	int32	充值蓝钻数

	}

	public class p_card_letter
	{
		public Int32 id = 0;	//required int32		唯一id

		public Int32 type = 0;	//required int32		贺卡类型

		public Boolean has_read = false;	//required bool		是否已读

		public Int32 recv_time = 0;	//required int32		时间戳

		public long from_id = 0;	//required int64		发送者id

		public string from_name = "";	//required string	发送人

		public long to_id = 0;	//required int64		收卡人id

		public string to_name = "";	//required string	收卡人

		public string text = "";	//required string 	内容

	}

	public class p_card_show_letter
	{
		public Int32 type = 0;	//required int32		贺卡类型

		public long from_id = 0;	//required int64		发送人id

		public string from_name = "";	//required string	发送人

		public long to_id = 0;	//required int64		收卡人id

		public string to_name = "";	//required string	收卡人

		public string text = "";	//required string 	内容

		public Int32 recv_time = 0;	//required int32		时间戳

	}

	public class p_ranking_silver
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public Int32 level = 0;	//required	int32 	等级

		public Int32 silver = 0;	//required	int32 	金币

	}

	public class p_letter_msg
	{
		public Int32 id = 0;	//required int32	id

		public string msg = "";	//optional string

	}

	public class p_title_msg
	{
		public Int32 id = 0;	//required int32	id

		public string msg = "";	//optional string

	}

	//匹配%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	public class p_ranking_role_pvp5
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public Int32 level = 0;	//required	int32 	等级

		public Int32 kill_num = 0;	//required	int32 	战力

		public Int32 win_times = 0;	//required	int32 	胜场

		public Int32 sum_times = 0;	//required	int32 	总场次

	}

	public class p_ranking_role_match_rank
	{
		public Int32 ranking = 0;	//required	int32 	排名

		public long role_id = 0;	//required	int64 	角色ID

		public string role_name = "";	//required	string  角色名

		public Int32 dan_grading = 0;	//optional	int32  	段位

		public Int32 star = 0;	//optional	int32   星数

		public Int32 time = 0;	//optional	int32   时间

	}

	public class p_pvp5_role_info
	{
		public long role_id = 0;	//required	int64	角色id

		public string role_name = "";	//required	string	角色名

		public Int32 vip_grade = 0;	//optional   int32

		public Int32 role_score = 0;	//optional	int32

		public Int32 level = 0;	//optional   int32

		public string head_addr = "";	//optional	string	头像路径

		public Int32 head_frame = 0;	//optional	int32	使用的头像框

		public Int32 dan_grading = 0;	//optional	int32  	段位

		public Int32 king_time = 0;	//optional	int32  	是否荣耀王者， 传的是时间戳，大于当前时间就是

		public Int32 server_id = 0;	//optional	int32	服务器id

		public Int32 corps_id = 0;	//optional	int32	战队ID

		public Int32 ai = 0;	//optional	int32	机器人AI

		public Int32 agent_id = 0;	//optional	int32

	}

	public class p_fight_info
	{
		public Int32 times = 0;	//optional	int32

		public Int32 win_times = 0;	//optional	int32

		public Int32 mvp_times = 0;	//optional	int32

		public Int32 dashen_num = 0;	//optional	int32

		public Int32 chaoshen_num = 0;	//optional   int32

		public Int32 kill3_num = 0;	//optional	int32

		public Int32 kill4_num = 0;	//optional	int32

		public Int32 kill5_num = 0;	//optional   int32

	}

	public class p_pvp5_fight_result
	{
		public Int32 hero_type = 0;	//optional	int32

		public Int32 fight_type = 1;	//optional	int32  对局模式

		public Boolean is_win = false;	//optional	bool

		public Int32 kill_num = 0;	//optional	int32

		public Int32 dead_num = 0;	//optional   int32

		public Int32 assists_num = 0;	//optional	int32

		public Int32 time = 0;	//optional	int32

		public Int32[] equips = null;	//repeated	int32

		public Int32 score = 0;	//required	int32 评分

		public Int32 rank = 0;	//required	int32 当局排名

		public Int32 fight_time = 0;	//required	int32

		public Int32 team_num = 0;	//required	int32

		public p_fight_result[] roles = null;	//repeated	p_fight_result

	}

	public class p_fight_hero_info
	{
		public Int32 hero_type = 0;	//optional	int32

		public Int32 times = 0;	//optional	int32  使用次数

		public Int32 win_times = 0;	//optional	int32  胜利次数

		public Int32 kill_num = 0;	//optional	int32

		public Int32 dead_num = 0;	//optional   int32

		public Int32 assists_num = 0;	//optional	int32

	}

	public class p_role_treasure_box
	{
		public Int32 pos_id = 0;	//optional   int32

		public Int32 type_id = 0;	//optional   int32

		public Int32 cool_time = 0;	//optional  	int32 0未开始解锁， 大于0表示解锁结束时间

	}

	public class p_egg_award
	{
		public Int32 index = 0;	//required int32 开启的位置

		public Int32 id = 0;	//required int32 前端用不到

		public Int32 type = 0;	//required int32 奖励类型5：道具

		public Int32 goods_id = 0;	//required int32 id

		public Int32 num = 0;	//required int32

	}

	public class p_treasure_award
	{
		public Int32 is_big = 0;	//required int32 是否大奖

		public Int32 type = 0;	//required int32 2:原石，3：金币，4：合金，5：道具，6：晶片,7:英雄

		public Int32 type_id = 0;	//required int32

		public Int32 num = 0;	//required int32

	}

	//-----------------------------------------------------------------------
	public class p_chip_page_pos
	{
		public Int32 page_id = 0;	//optional    int32   芯片 ID

		public Int32 pos = 0;	//required 	 int32   位置

	}

	public class p_chip_info
	{
		public Int32 id = 0;	//required	int32  自增id

		public Int32 type_id = 0;	//optional   int32   芯片 ID

		public Int32 num = 0;	//required	int32,

		public Int32 suit_id = 0;	//required	int32 套装id

		public p_kvi[] prop_c = null;	//repeated   p_kvi 前端无用，后端用来重算属性的

		public p_kvi[] prop = null;	//repeated   p_kvi

		public Int32[] load_page_list = null;	//repeated 	 int32

	}

	public class p_chip_pos
	{
		public Int32 pos_id = 0;	//required    int32   位置 1到30

		public Int32 id = 0;	//optional    int32   芯片 ID

	}

	public class p_chip_page
	{
		public Int32 id = 0;	//required    int32   芯片页码 1,2,3,4,5,6,7,8,9,10

		public string page_name = "";	//optional    string

		public p_chip_pos[] chip_list = null;	//repeated    p_chip_pos   芯片

		public Int32 sum_level = 0;	//optional    int32   总等级

		public p_kvi[] prop_list = null;	//repeated    p_kvi	 属性

		public p_kvi[] skill = null;	//repeated	 p_kvi 技能id

	}

}

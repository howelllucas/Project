//物品数据对象
public class ItemDTO
{
    //物品id
    public int itemId;
    //数量
    public double num;
    //类型(业务类型，用于日志记录)
    public int type;
    //结果是否成功
    public bool result = true;
    //日志参数1
    public double param1;
    //日志参数2
    public double param2;
    //日志参数3
    public double param3;
    //日志字符串参数1
    public string paramStr1;
    //日志字符串参数2
    public string paramStr2;
    //日志字符串参数3
    public string paramStr3;
    //操作后的数量
    public double after;
    public ItemDTO()
    {
    }

    public ItemDTO(int itemId, double num, int type)
    {
        this.itemId = itemId;
        this.num = num;
        this.type = type;
    }

}

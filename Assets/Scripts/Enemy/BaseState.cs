//有限状态机-抽象类
public abstract class BaseState //抽象类
{
    protected Enemy currentEnemy;   //创建一个protected（可被子类访问）Enemy类型的currentEnemy这样各种状态在继承时就不用在创建了
    public abstract void OnEnter(Enemy enemy);  //进入时获取当前enemy是什么
    public abstract void LogicUpdate(); //逻辑判断
    public abstract void PhysicsUpdate();   //物理逻辑判断
    public abstract void OnExit();
}

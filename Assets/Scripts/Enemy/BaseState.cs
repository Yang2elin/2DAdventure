//有限状态机-抽象类
public abstract class BaseState //抽象类
{
    protected Enemy currentEnemy;
    public abstract void OnEnter(Enemy enemy);  //进入时获取当前enemy是什么
    public abstract void LogicUpdate(); //逻辑判断
    public abstract void PhysicsUpdate();   //物理逻辑判断
    public abstract void OnExit();
}

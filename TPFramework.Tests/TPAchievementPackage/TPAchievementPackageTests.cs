using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPFramework.Core;

namespace TPFramework.Tests
{
    [TestClass]
    public class TPAchievementPackageTests
    {
        [TestMethod]
        public void Complete()
        {
            TPAchievementData data = new TPAchievementData() {
                ReachPoints = 10
            };
            TPAchievement<TPAchievementData> achievement = new TPAchievement<TPAchievementData>(data);
            achievement.Complete();
            data.IsCompleted = true;
            Assert.IsTrue(achievement.Data.IsCompleted);
        }

        [TestMethod]
        public void AddPoints()
        {
            TPAchievementData data = new TPAchievementData() {
                ReachPoints = 10
            };
            TPAchievement<TPAchievementData> achievement = new TPAchievement<TPAchievementData>(data);

            achievement.AddPoints();
            Assert.AreEqual(1, achievement.Data.Points);
            achievement.AddPoints(5);
            Assert.AreEqual(6, achievement.Data.Points);
            achievement.AddPoints(555);
            Assert.AreEqual(10, achievement.Data.Points);
        }
    }
}

using Geekium.Controllers;
using Geekium.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestGeekium
{
	public class RewardUnitTests
	{
		GeekiumContext context = new GeekiumContext();
		IWebHostEnvironment hostEnvironment;
		Reward reward;

		private void Initialize()
		{
			try { context.Entry(reward).State = EntityState.Detached; }
			catch (Exception) { }

			reward = new Reward
			{
				RewardId = 1,
				AccountId = 12345,
				RewardType = "Placeholder",
				RewardCode = "123456",
				PointCost = 25,
				DateReceived = DateTime.Now
			};
		}

		[Fact]

		public async Task TwentyFivePercentDiscountId_ShouldAllowCreate()
		{
			//Arrange
			Initialize();
			var rewardsController = new RewardsController(context, hostEnvironment);
			var account = await context.Accounts.FirstOrDefaultAsync(m => m.AccountId == 2);
			reward.RewardId = 1;

			//Act
			await rewardsController.AddRewardToAccount(reward.RewardId, account);

			//Assert
			Assert.True(rewardsController.ModelState.IsValid);
		}

		[Fact]
		public async Task FiftyPercentDiscountId_ShouldAllowCreate()
		{
			//Arrange
			Initialize();
			var rewardsController = new RewardsController(context, hostEnvironment);
			var account = await context.Accounts.FirstOrDefaultAsync(m => m.AccountId == 2);
			reward.RewardId = 2;

			//Act
			await rewardsController.AddRewardToAccount(reward.RewardId, account);

			//Assert
			Assert.True(rewardsController.ModelState.IsValid);
		}

		[Fact]
		public async Task SeventyFivePercentDiscountId_ShouldAllowCreate()
		{
			//Arrange
			Initialize();
			var rewardsController = new RewardsController(context, hostEnvironment);
			var account = await context.Accounts.FirstOrDefaultAsync(m => m.AccountId == 2);
			reward.RewardId = 3;

			//Act
			await rewardsController.AddRewardToAccount(reward.RewardId, account);

			//Assert
			Assert.True(rewardsController.ModelState.IsValid);
		}
	}
}

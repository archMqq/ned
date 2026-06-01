using EntryPointAPI.Models;
using EntryPointAPI.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntryPointTests.Validators
{
    public class NotificationDataValidationTests
    {
        [Fact]
        public void Validate_ND_UserId_Invalid()
        {
            var notificationData = new NotificationData();
            var type = notificationData.GetType();
            var field = type.GetField("<Channels>k__BackingField", System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance);
            field.SetValue(notificationData, new Channel[] { new Channel { Type = ChannelType.Email, Address = "email" } });

            var validator = new NotificationDataValidator();
            var res = validator.Validate(notificationData);
            
            Assert.False(res.IsValid);
            Assert.True(res.Errors.Count() == 1);
            Assert.Contains("greater than 0", res.Errors[0]);
        }
        
        [Fact]
        public void Validate_ND_No_Channels()
        {
            var notificationData = new NotificationData();
            var type = notificationData.GetType();
            var field = type.GetField("<UserId>k__BackingField", System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance);
            field.SetValue(notificationData, 1);

            var validator = new NotificationDataValidator();
            var res = validator.Validate(notificationData);

            Assert.False(res.IsValid);
            Assert.True(res.Errors.Count() == 1);
            Assert.Contains("channel", res.Errors[0]);
        }

        [Fact]
        public void Validate_ND_Channels_No_Data()
        {
            var notificationData = new NotificationData();
            var type = notificationData.GetType();
            var field = type.GetField("<UserId>k__BackingField", System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance);
            field.SetValue(notificationData, 1);

            field = type.GetField("<Channels>k__BackingField", System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance);
            field.SetValue(notificationData, new Channel[] { new Channel { Type = ChannelType.Email } });


            var validator = new NotificationDataValidator();
            var res = validator.Validate(notificationData);

            Assert.False(res.IsValid);
            Assert.True(res.Errors.Count() == 1);
            Assert.Contains("empty", res.Errors[0]);
        }

        [Fact]
        public void Validate_ND_Full_Invalid()
        {
            var notificationData = new NotificationData();
            var type = notificationData.GetType();

            var validator = new NotificationDataValidator();
            var res = validator.Validate(notificationData);

            Assert.False(res.IsValid);
            Assert.True(res.Errors.Count() == 2);
            Assert.Contains("greater than 0", res.Errors[0]);
            Assert.Contains("channel", res.Errors[1]);
        }

        [Fact]
        public void Validate_ND_Valid()
        {
            var notificationData = new NotificationData();
            var type = notificationData.GetType();
            var field = type.GetField("<UserId>k__BackingField", System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance);
            field.SetValue(notificationData, 1);
            field = type.GetField("<Channels>k__BackingField", System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance);
            field.SetValue(notificationData, new Channel[] { new Channel { Type = ChannelType.Email, Address = "email" } });

            var validator = new NotificationDataValidator();
            var res = validator.Validate(notificationData);

            Assert.True(res.IsValid);
        }
    }
}

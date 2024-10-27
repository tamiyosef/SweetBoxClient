using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace SweetBoxApp.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior), null);

        private EventInfo eventInfo;
        private Delegate eventHandler;

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
                throw new ArgumentException($"EventToCommandBehavior: Can't register the '{EventName}' event.");

            var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent));
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        private void DeregisterEvent(string name)
        {
            if (eventInfo == null)
                return;

            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventInfo = null;
            eventHandler = null;
        }

        private void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
                return;

            var resolvedParameter = CommandParameter ?? eventArgs;

            if (Command.CanExecute(resolvedParameter))
                Command.Execute(resolvedParameter);
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;

            if (behavior.AssociatedObject == null)
                return;

            behavior.DeregisterEvent((string)oldValue);
            behavior.RegisterEvent((string)newValue);
        }
    }

    public abstract class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            AssociatedObject = null;
        }
    }
}

using NHibernate;
using NHibernate.Event;
using NHibernate.Event.Default;
using System;

public class MyEventListener : DefaultSaveOrUpdateEventListener
{
    protected override object PerformSaveOrUpdate(SaveOrUpdateEvent @event)
    {
        // Fügen Sie hier Ihre benutzerdefinierte Logik hinzu
        Console.WriteLine("Entity saved or updated: " + @event.Entity);
        return base.PerformSaveOrUpdate(@event);
    }
}

public class NHibernateHelper
{
    private static ISessionFactory _sessionFactory;

    public static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
            {
                var configuration = new NHibernate.Cfg.Configuration();
                configuration.Configure(); // Konfigurationsdatei laden (hibernate.cfg.xml)

                // Event-Listener hinzufügen
                configuration.SetListener(ListenerType., new MyEventListener());

                _sessionFactory = configuration.BuildSessionFactory();
            }
            return _sessionFactory;
        }
    }

    public static ISession OpenSession()
    {
        return SessionFactory.OpenSession();
    }
}

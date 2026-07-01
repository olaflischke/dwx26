using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Cfg;

using ChinookDal;
using NHibernate.Linq;

namespace ChinookUi
{
    public partial class Form1 : Form
    {
        Configuration configuration;
        ISessionFactory sessionFactory;

        public Form1()
        {
            InitializeComponent();

            configuration = ConfigureNHibernate();
            sessionFactory = configuration.BuildSessionFactory();
        }

        private static Configuration ConfigureNHibernate()
        {
            var config = new Configuration();

            config.DataBaseIntegration(db =>
            {
                db.ConnectionString = Properties.Settings.Default.ChinookConString;
                db.Driver<NHibernate.Driver.SqlClientDriver>();
                db.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
                db.IsolationLevel = IsolationLevel.RepeatableRead;
                db.LogSqlInConsole = true;
                db.Timeout = 10;
                db.BatchSize = 10;
            });


            config.AddAssembly("ChinookDal");

            return config;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var qGenres = session.Query<Genre>();

                foreach (Genre item in qGenres)
                {
                    TreeNode node = new TreeNode(item.Name);
                    node.Tag = item;
                    treeView1.Nodes.Add(node);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            Genre genre = node.Tag as Genre;

            if (genre != null)
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    var qTracks = session.Query<Track>()
                                            .Fetch(tr => tr.Album).ThenFetch(al => al.Artist)
                                            .Fetch(tr => tr.Genre)
                                            .Where(tr => tr.Genre.GenreId == genre.GenreId);
                    //var qTracksMitS = qTracks.Where(t => t.Name.StartsWith("S"));

                    // Deferred Execution
                    dataGridView1.DataSource = qTracks.ToList();
                }
            }

            
        }
    }
}

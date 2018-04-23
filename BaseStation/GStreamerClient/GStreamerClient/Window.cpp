
extern "C" {
	#include <gtk/gtk.h>
	#include <stdlib.h>
}

static char *filename;

static void
activate(GtkApplication *app, gpointer p)
{

	GtkWidget *window;
	GtkBuilder *builder;

	/* create a new window, and set its title */
	builder = gtk_builder_new();
	gtk_builder_add_from_file(builder, "main.ui", NULL);
	window = GTK_WIDGET(gtk_builder_get_object(builder, "window1"));
	gtk_application_add_window(app, GTK_WINDOW(window));

	gtk_window_set_title(GTK_WINDOW(window), "Video Player");
	gtk_container_set_border_width(GTK_CONTAINER(window), 10);

	g_object_unref(builder);

	gtk_widget_show_all(window);
}

int main(int argc, char *argv[])
{
	GtkApplication *app;
	int status;

	filename = argv[1];

	app = gtk_application_new(NULL, G_APPLICATION_HANDLES_OPEN);
	g_signal_connect(app, "activate", G_CALLBACK(activate), NULL);

	status = g_application_run(G_APPLICATION(app), argc, argv);
	g_object_unref(app);

	return status;
}

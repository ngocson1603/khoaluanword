# Generated by Django 4.1.4 on 2022-12-20 07:54

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('employees', '0001_initial'),
    ]

    operations = [
        migrations.AddField(
            model_name='employees',
            name='avatar',
            field=models.ImageField(default=None, upload_to='images'),
        ),
        migrations.AddField(
            model_name='employees',
            name='cv',
            field=models.FileField(default=None, upload_to='files'),
        ),
    ]

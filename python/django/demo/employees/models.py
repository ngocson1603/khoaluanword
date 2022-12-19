from django.db import models
from home.models import department

# Create your models here.


class employees(models.Model):
    employee_id = models.AutoField(primary_key=True)
    department_id = models.ForeignKey(
        department, default=None, on_delete=models.CASCADE)
    name = models.CharField(max_length=200, null=False)
    age = models.IntegerField(null=True)

    def __str__(self):
        return f"{self.employee_id},{self.department_id},{self.name},{self.age}"

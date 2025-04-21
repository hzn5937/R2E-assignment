import React from 'react';
import { Button, Card } from 'antd';
import { useNavigate } from 'react-router-dom';

const Home = () => {
  const navigate = useNavigate();
  const totalEmployees = 42;

  return (
    <div className="flex flex-col items-center justify-center h-full mt-20 space-y-6">
        <h1 className="text-4xl font-bold text-blue-700">🏢 Employee Management System</h1>

        <Card className="shadow-lg w-80 text-center">
            <p className="text-xl font-medium text-gray-700">Total Employees</p>
            <p className="text-3xl font-bold text-green-600 mt-2">{totalEmployees}</p>
        </Card>
        
        <div className='mt-5'>
            <Button
                type="primary"
                size="large"
                onClick={() => navigate('/post')}
            >
                Manage Employees
            </Button>
        </div>

    </div>
  );
};

export default Home;

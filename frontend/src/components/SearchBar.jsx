import React from 'react';
import { Input, Button } from 'antd';
import { SearchOutlined } from '@ant-design/icons';

const SearchBar = ({ 
  searchTerm,
  setSearchTerm,
  handleSearch,
  handleClearSearch,
  searchLoading,
  placeholder = 'Search by title, author, or category...'
}) => {
  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
  };

  // Handle enter key press in search input
  const handleSearchKeyPress = (e) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <div className="mb-4 flex">
      <Input
        placeholder={placeholder}
        value={searchTerm}
        onChange={handleSearchChange}
        onKeyPress={handleSearchKeyPress}
        style={{ width: '60%' }}
        prefix={<SearchOutlined />}
        allowClear
      />
      <div className="ml-2 flex items-center">
        <Button 
            type="primary" 
            onClick={handleSearch} 
            loading={searchLoading}
        >
            Search
        </Button>
      </div>
      
      <div className="ml-2 flex items-center">
        {searchTerm && (
            <Button onClick={handleClearSearch} className="ml-2">
            Clear
            </Button>
        )}
      </div>
    </div>
  );
};

export default SearchBar;
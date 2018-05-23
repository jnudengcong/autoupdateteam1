﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoUpdate
{
    class ConFile
    {
        private string name;
        private float version;
        private string time;
        private string hash;
        private int file_count;
        private List<FileInfo> file_list;

        ConFile(string name, float version, string time, string hash, int file_count)
        {
            this.name = name;
            this.version = version;
            this.time = time;
            this.hash = hash;
            this.file_count = file_count;
        }
        
        ConFile(string name)
        {
            this.name = name;
            ReadConFile();
        }

        // 生成配置文件
        public void SaveConFile()
        {
            FileStream fs = new FileStream(this.name, FileMode.Create, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.Flush();
            // 设置当前流的位置
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            // 写入配置文件自身的信息
            m_streamWriter.Write(this.version.ToString() + "\n");
            m_streamWriter.Write(this.time.ToString() + "\n");
            m_streamWriter.Write(this.hash.ToString() + "\n");
            m_streamWriter.Write(this.file_count.ToString() + "\n");
            
            foreach (var item in file_list)
            {
                // 写入配置文件列表中的文件信息
                m_streamWriter.Write(item.GetName().ToString() + "\n");
                m_streamWriter.Write(item.GetVersion().ToString() + "\n");
                m_streamWriter.Write(item.GetHash().ToString() + "\n");
                m_streamWriter.Write(item.GetUpdateMethod().ToString() + "\n");
            }
            //关闭此文件
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }
        
        // 读取配置文件
        public void ReadConFile()
        {
            FileStream fs = new FileStream(this.name, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader m_streamReader = new StreamReader(fs);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            // 读取配置文件自身的信息
            this.version = float.Parse(m_streamReader.ReadLine());
            this.time = m_streamReader.ReadLine();
            this.hash = m_streamReader.ReadLine();
            this.file_count = int.Parse(m_streamReader.ReadLine());

            for (int i = 0; i < file_count; i++)
            {
                // 读取配置文件列表中的文件信息
                string name = m_streamReader.ReadLine();
                float version = float.Parse(m_streamReader.ReadLine());
                string hash = m_streamReader.ReadLine();
                // 先转为int，再强制转换为enum类型
                FileInfo.UpdateMethod update_method = (FileInfo.UpdateMethod)int.Parse(m_streamReader.ReadLine());
                file_list.Add(new FileInfo(name, version, hash, update_method));
            }

            //string strLine = m_streamReader.ReadLine();
            m_streamReader.Close();
        }


        public string GetName()
        {
            return this.name;
        }

        public float GetVersion()
        {
            return this.version;
        }

        public string GetTime()
        {
            return this.time;
        }

        public string GetHash()
        {
            return this.hash;
        }

        public int GetFileCount()
        {
            return this.file_count;
        }
    }
}
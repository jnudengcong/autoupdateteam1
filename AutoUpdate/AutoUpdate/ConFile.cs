﻿using System;
using System.Collections.Generic;
using System.IO;


namespace AutoUpdate
{
    public class ConFile
    {
        private string name;
        private float version;
        private string time;
        private string hash;
        private List<ProjectFile> file_list;

        public ConFile(string name)
        {
            this.name = name;
            file_list = new List<ProjectFile>();
            if (System.IO.File.Exists(name))
            {
                ReadConFile();
            }
            else
            {
                AppInfo app_info = AppInfo.GetInstance();
                version = app_info.GetVersion() + 0.01f;
                time = DateTime.Now.ToString("yyyy/MM/dd");
            }
      
        }


        public void AddFile(ProjectFile project_info)
        {
            file_list.Add(project_info);
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
            m_streamWriter.Write(this.file_list.Count.ToString() + "\n");

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
            int file_count = int.Parse(m_streamReader.ReadLine());

            for (int i = 0; i < file_count; i++)
            {
                // 读取配置文件列表中的文件信息
                string name = m_streamReader.ReadLine();
                float version = float.Parse(m_streamReader.ReadLine());
                string hash = m_streamReader.ReadLine();
                // 先转为int，再强制转换为enum类型
                ProjectFile.UpdateMethod update_method = (ProjectFile.UpdateMethod)Enum.Parse(typeof(ProjectFile.UpdateMethod), m_streamReader.ReadLine());
                file_list.Add(new ProjectFile(name, version, hash, update_method));
            }

            //string strLine = m_streamReader.ReadLine();
            m_streamReader.Close();
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return this.name;
        }

        public string GetPackageName()
        {
            return "aus_v" + version.ToString();
        }

        public void SetVersion(float version)
        {
            this.version = version;
        }

        public float GetVersion()
        {
            return this.version;
        }

        public void SetTime(string time)
        {
            this.time = time;
        }

        public string GetTime()
        {
            return this.time;
        }

        public void SetHash(string hash)
        {
            this.hash = hash;
        }

        public string GetHash()
        {
            return this.hash;
        }

        public int GetFileCount()
        {
            return this.file_list.Count;
        }

        public List<ProjectFile> GetList()
        {
            return file_list;
        }
    }
}

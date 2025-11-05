<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>


      <xsl:variable name = "varDate">
        <xsl:value-of select="//PositionMaster[substring(COL1,11,6)='BALTNV']/COL1"/>
      </xsl:variable>

      <xsl:for-each select="//PositionMaster">


		  <xsl:if test=" substring(COL1,3,8)='19210400'or substring(COL1,3,8)='19210327'">

          <PositionMaster>

            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="substring(COL1,3,8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select="''"/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <LocalCurrency>
              <xsl:value-of select="'USD'"/>
            </LocalCurrency>

            <xsl:choose>
              <xsl:when test ="substring(COL1,49,1)= '+'">
                <CashValueBase>
                  <xsl:value-of select="substring(COL1,50,15)"/>
                </CashValueBase>
              </xsl:when >
              <xsl:when test ="substring(COL1,49,1)= '-'">
                <CashValueBase>
                  <xsl:value-of select="substring(COL1,50,15)*(-1)"/>
                </CashValueBase>
              </xsl:when >
              <xsl:otherwise>
                <CashValueBase>
                  <xsl:value-of select="0"/>
                </CashValueBase>
              </xsl:otherwise>
            </xsl:choose >

            <xsl:choose>
              <xsl:when test ="substring(COL1,49,1)= '+'">
                <CashValueLocal>
                  <xsl:value-of select="substring(COL1,50,15)"/>
                </CashValueLocal>
              </xsl:when >
              <xsl:when test ="substring(COL1,49,1)= '-'">
                <CashValueLocal>
                  <xsl:value-of select="substring(COL1,50,15)*(-1)"/>
                </CashValueLocal>
              </xsl:when >
              <xsl:otherwise>
                <CashValueLocal>
                  <xsl:value-of select="0"/>
                </CashValueLocal>
              </xsl:otherwise>
            </xsl:choose >


            <xsl:choose>
              <xsl:when test ="$varDate != ''">
                <Date>
                  <xsl:value-of select="concat(substring($varDate,7,2),'/',substring($varDate,9,2),'/',substring($varDate,3,4))"/>
                </Date>
              </xsl:when>
              <xsl:otherwise>
                <Date>
                  <xsl:value-of select="''"/>
                </Date>
              </xsl:otherwise>
            </xsl:choose>
            

            <PositionType>
              <xsl:value-of select="'Cash'"/>
            </PositionType>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>

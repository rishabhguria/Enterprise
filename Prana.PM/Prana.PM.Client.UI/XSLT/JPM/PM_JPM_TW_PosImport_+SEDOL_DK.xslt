<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

  <xsl:template name="GetMonth">
    <xsl:param name="varMonthNo"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonthNo='01' and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='02' and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='03' and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='04' and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='05' and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='06' and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='07' and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='08' and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='09' and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='10' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='11' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='12' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='01' and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='02' and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='03' and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='04' and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='05' and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='06' and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='07' and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='08' and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='09' and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='10' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='11' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonthNo='12' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Symbol">
    <xsl:param name="varCurrency"/>
    <xsl:choose>
      <xsl:when test="$varCurrency = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'DKK'">
        <xsl:value-of select="'-OMX'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'GBP'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'HKD'">
        <xsl:value-of select="'-HKG'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$varCurrency = 'SGD'">
        <xsl:value-of select="'-SES'"/>
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="number(COL21)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='DB']/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <FundName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>

            <PositionStartDate>
              <xsl:value-of select="COL11"/>
            </PositionStartDate>

            <xsl:choose>
             
              <xsl:when test="normalize-space(COL4)='OPTIONS'">

                <xsl:variable name="varExpirationDate">
                  <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL10),' '),' '),' ')"/>
                </xsl:variable>
                
                <xsl:variable name="varYear">
                  <xsl:value-of select="substring($varExpirationDate,7,2)"/>
                </xsl:variable>

                <xsl:variable name="varMonth">
                  <xsl:value-of select="substring($varExpirationDate,1,2)"/>
                </xsl:variable>

                <xsl:variable name="varDateNo">
                  <xsl:value-of select="substring($varExpirationDate,4,2)"/>
                </xsl:variable>
                
                <xsl:variable name="varThirdFriday">
                  <xsl:value-of select =" my:Now(number(concat('20',$varYear)),number($varMonth))"/>
                </xsl:variable>

               
                <xsl:variable name="varIsFlex">
                  <xsl:choose>
                    <xsl:when test="(substring-before(substring-after($varThirdFriday,'/'),'/') + 1) = number($varDateNo)">
                      <xsl:value-of select="0"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="1"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="varDate">
                  <xsl:choose>
                    <xsl:when test="substring(COL27,4,1)='0'">
                      <xsl:value-of select="substring(COL27,5,2)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring(COL27,4,2)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="MonthCode">
                  <xsl:call-template name="GetMonth">
                    <xsl:with-param name="varMonthNo" select="$varMonth"/>
                    <xsl:with-param name="varPutCall" select="COL26"/>
                  </xsl:call-template>
                </xsl:variable>
                <Symbol>
                  <xsl:choose>
                    <xsl:when test="$varIsFlex = 0">
                      <xsl:value-of select="concat('O:',normalize-space(COL16),' ',$varYear,$MonthCode,format-number(COL28,'#.00'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat('O:',normalize-space(COL16),' ',$varYear,$MonthCode,format-number(COL28,'#.00'),'D',$varDate)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Symbol>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:variable name="varSuffix">
                  <xsl:call-template name="Symbol">
                    <xsl:with-param name="varCurrency" select="COL3"/>
                  </xsl:call-template>
                </xsl:variable>
                <Symbol>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME = ''">
                      <xsl:choose>
                        <xsl:when test="string-length(normalize-space(COL12))=4 and (COL3 = 'HKD' or COL3 = 'SGD' or COL3 = 'JPY')">
                          <xsl:value-of select="concat(COL12,$varSuffix)"/>
                        </xsl:when>
                        <xsl:when test="string-length(normalize-space(COL12))=3 and (COL3 = 'HKD' or COL3 = 'SGD' or COL3 = 'JPY')">
                          <xsl:value-of select="concat('0',COL12,$varSuffix)"/>
                        </xsl:when>
                        <xsl:when test="string-length(normalize-space(COL12))=2 and (COL3 = 'HKD' or COL3 = 'SGD' or COL3 = 'JPY')">
                          <xsl:value-of select="concat('00',COL12,$varSuffix)"/>
                        </xsl:when>
						  <xsl:when test ="COL3 = 'CAD' and contains(COL12,'/') != false">
							  <xsl:value-of select ="concat(substring-before(COL12,'/'),'.',substring-after(COL12,'/'),$varSuffix)"/>
						  </xsl:when>
						  <xsl:when test ="normalize-space(COL4)='FIXED INCOME'">
							  <xsl:value-of select ="translate(normalize-space(COL15),$varLower,$varUpper)"/>
						  </xsl:when>
						  
						  <xsl:otherwise>
							  <xsl:value-of select ="concat(COL12,$varSuffix)"/>
						  </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_Symbol_NAME"/>
                    </xsl:otherwise>
                   
                  </xsl:choose>
                </Symbol>  
              </xsl:otherwise>
              
            </xsl:choose>


            <PBSymbol>
              <xsl:value-of select="COL10"/>
            </PBSymbol>

            <!--<PBAssetType>
              <xsl:value-of select ="COL5"/>
            </PBAssetType>-->

            <!--QUANTITY-->


            <NetPosition>
              <xsl:choose>
                <xsl:when test="COL21 &lt; 0">
                  <xsl:value-of select="COL21 * (-1)"/>
                </xsl:when>
                <xsl:when test="COL21 &gt; 0">
                  <xsl:value-of select="COL21"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <!--Side-->

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="COL21 &lt; 0">
                  <xsl:value-of select="2"/>
                </xsl:when>
                <xsl:when test="COL21 &gt; 0">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>            
            </SideTagValue>


            <xsl:choose>
              <xsl:when test ="boolean(number(COL22))">
                <CostBasis>
                  <xsl:value-of select="COL22"/>
                </CostBasis>
              </xsl:when>
              <xsl:otherwise>
                <CostBasis>
                  <xsl:value-of select="0"/>
                </CostBasis>
              </xsl:otherwise>
            </xsl:choose>

           
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


	<xsl:variable name ="varLower">abcdefghijklmnopqrstuvwxyz</xsl:variable>
		<xsl:variable name ="varUpper">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
	
</xsl:stylesheet>


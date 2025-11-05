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


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="varCostBasis">
          <xsl:value-of select="COL15"/>
        </xsl:variable>

        <xsl:if test ="number(COL14)">

          <xsl:variable name="varPBName">
            <xsl:value-of select="'Jefferies'"/>
          </xsl:variable>

          <PositionMaster>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL5"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='IB']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="varAssetType">
				  <xsl:value-of select="COL2"/>
			  </xsl:variable>

			  <xsl:variable name="varOpenClose">
				  <xsl:value-of select="COL21"/>
			  </xsl:variable>
			  
            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL14"/>
            </xsl:variable>


            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="COL2 = 'OPT'">
                  <xsl:value-of select="''"/>
                </xsl:when>
				  <xsl:when test="COL2='FUT'">
					  <xsl:value-of select ="normalize-space(concat(substring(COL4,1,2),' ',substring(COL4,3,2)))"/>
				  </xsl:when>
				  <xsl:when test="COL2='FOP'">
					  <xsl:value-of select ="normalize-space(concat(substring(COL4,1,2),' ',substring(COL4,3,2),substring(COL4,6)))"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL4"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="COL2 = 'OPT'">
                  <xsl:value-of select="concat(COL4,'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>


            <AvgPx>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCostBasis))">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPx>


            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="number($varNetPosition)">
                <Quantity>
                  <xsl:value-of select="$varNetPosition"/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <Side>

				<xsl:choose>
					<xsl:when test="$varNetPosition &gt; 0 and $varAssetType = 'OPT' and $varOpenClose = 'O'">
						<xsl:value-of select="'Buy to Open'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &lt; 0 and $varAssetType = 'OPT' and $varOpenClose = 'O'">
						<xsl:value-of select="'Sell to Open'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &gt; 0 and $varAssetType = 'OPT' and $varOpenClose = 'C'">
						<xsl:value-of select="'Buy to Close'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &lt; 0 and $varAssetType = 'OPT' and $varOpenClose = 'C'">
						<xsl:value-of select="'Sell to Close'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &gt; 0 and (($varAssetType = 'STK' and ($varOpenClose = 'C;O' or $varOpenClose = 'O')) or $varAssetType = 'FUT')">
						<xsl:value-of select="'Buy'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &gt; 0 and ($varAssetType = 'STK' and $varOpenClose = 'C')">
						<xsl:value-of select="'Buy to Close'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &lt; 0 and (($varAssetType = 'STK' and $varOpenClose = 'C') or $varAssetType = 'FUT')">
						<xsl:value-of select="'Sell'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &lt; 0 and (($varAssetType = 'STK' and $varOpenClose = 'O') )">
						<xsl:value-of select="'Sell short'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &gt; 0 and ($varAssetType = 'STK' or $varAssetType = 'FOP')">
						<xsl:value-of select="'Buy'"/>
					</xsl:when>
					<xsl:when test="$varNetPosition &lt; 0 and ($varAssetType = 'STK' or $varAssetType = 'FOP')">
						<xsl:value-of select="'Sell'"/>
					</xsl:when>
					<!--<xsl:when test="$varSide = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'BC'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>-->
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
             
            </Side>

			  <xsl:variable name="NetNotional">
				  <xsl:choose>
					  <xsl:when test="COL2='FUT'">
						  <xsl:value-of select="number((COL15 * COL16 * COL14)) - number(COL19)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="number(COL28)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <NetNotionalValue>
              <xsl:choose>
				  <xsl:when test="$NetNotional &gt; 0">
					  <xsl:value-of select="$NetNotional"/>
				  </xsl:when>
                <xsl:when test="$NetNotional &lt; 0">
                  <xsl:value-of select="$NetNotional * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

			  <xsl:variable name="NetNotionalBase">
				  <xsl:value-of select="$NetNotional*COL10"/>
			  </xsl:variable>

			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalBase &gt; 0">
						  <xsl:value-of select="$NetNotionalBase"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalBase &lt; 0">
						  <xsl:value-of select="$NetNotionalBase * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>

            <SMRequest>
              <xsl:value-of select="'TRUE'"/>
            </SMRequest>
			  <Commission>
				  <xsl:choose>
					  <xsl:when test="COL19 &gt; 0">
						  <xsl:value-of select="COL19"/>
					  </xsl:when>
					  <xsl:when test="COL19 &lt; 0">
						  <xsl:value-of select="COL19*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Commission>
			  <TotalCommissionandfees>
				  <xsl:choose>
					  <xsl:when test="COL19 &gt; 0">
						  <xsl:value-of select="COL19"/>
					  </xsl:when>
					  <xsl:when test="COL19 &lt; 0">
						  <xsl:value-of select="COL19*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </TotalCommissionandfees>
						
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
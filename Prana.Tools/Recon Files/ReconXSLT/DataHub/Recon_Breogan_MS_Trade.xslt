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
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test ="$varMonth='Jan'">
        <xsl:value-of select ="1"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Feb'">
        <xsl:value-of select ="2"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Mar' ">
        <xsl:value-of select ="3"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Apr'">
        <xsl:value-of select ="4"/>
      </xsl:when>
      <xsl:when test ="$varMonth='May'">
        <xsl:value-of select ="5"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Jun'">
        <xsl:value-of select ="6"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Jul'">
        <xsl:value-of select ="7"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Aug'">
        <xsl:value-of select ="8"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Sep' ">
        <xsl:value-of select ="9"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Oct'">
        <xsl:value-of select ="10"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Nov' ">
        <xsl:value-of select ="11"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Dec'">
        <xsl:value-of select ="12"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>


	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'FP'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'NA'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'BZ'">
				<xsl:value-of select="'-BSP'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'LN'">
				<xsl:value-of select="'-LON'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring(substring-before(substring-after(substring-after(substring-after(COL121,' '),' '),' '),' '),1,1),'P') or contains(substring(substring-before(substring-after(substring-after(substring-after(COL121,' '),' '),' '),' '),1,1),'C')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL121,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL121,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL121),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL121,'/'),'/'),' ')"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(COL121,' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(COL121,' '),' '),' '),' '),2),'#.00')"/>
			</xsl:variable>
			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
		</xsl:if>
	</xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:if test="COL26 = 'Trade' and contains(COL9, 'CANCEL') = false and COL95= ''">
 
          <xsl:variable name="varPBName">
            <xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
          </xsl:variable>

          <PositionMaster>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <!--<xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>-->
			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Morgan Stanley and Co. International plc']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>
			  
			  
			 
				  <FundName>
					  <xsl:choose>
						  <xsl:when test="$PRANA_FUND_NAME!=''">
							  <xsl:value-of select="$PRANA_FUND_NAME"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="$PB_FUND_NAME"/>
						  </xsl:otherwise>
						  </xsl:choose>	  
					  </FundName>
				 
			  
			  <xsl:variable name="PB_Symbol" select="COL15"/>
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
			  </xsl:variable>

			  <xsl:variable name="varSuffix">
				  <xsl:call-template name="GetSuffix">
					  <xsl:with-param name="Suffix" select="substring-after(COL25, ' ')"/>
				  </xsl:call-template>
			  </xsl:variable>


            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

			  <xsl:variable name="varUnderlying">
				  <xsl:if test="substring-before(COL84, ' ') = 'Call' or substring-before(COL84, ' ') = 'Put'">
					  <xsl:value-of select="substring-before(COL19, '1')"/>
				  </xsl:if>
			  </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
            </xsl:variable>

			  <xsl:variable name="varBlanks">
				  <xsl:if test="substring-before(COL84, ' ') = 'Call' or substring-before(COL84, ' ') = 'Put'">
					  <xsl:call-template name="noofBlanks">
						  <xsl:with-param name="count1" select="6-(string-length($varUnderlying))"/>
					  </xsl:call-template>
				  </xsl:if>
			  </xsl:variable>

			  <xsl:variable name="varIDCO">
				  <xsl:if test="substring-before(COL84, ' ') = 'Call' or substring-before(COL84, ' ') = 'Put'">
					  <xsl:value-of select="concat($varUnderlying, $varBlanks, substring(COL19, 1+(string-length($varUnderlying))),'U')"/>
				  </xsl:if>
			  </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL34"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:value-of select="COL71"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varClearingFee">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varStampDuty">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varTransactionLevy">
              <xsl:value-of select="0"/>
            </xsl:variable>

            
			  <Symbol>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
						  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>
					  <xsl:when test="contains(substring(substring-before(substring-after(substring-after(substring-after(COL121,' '),' '),' '),' '),1,1),'P') or contains(substring(substring-before(substring-after(substring-after(substring-after(COL121,' '),' '),' '),' '),1,1),'C')">
						  <xsl:call-template name="Option">
							  <xsl:with-param name="Symbol" select="COL121"/>
							  <xsl:with-param name="Suffix" select="''"/>
						  </xsl:call-template>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test='$varSuffix = "" and substring-after(COL25, " ") != "US"'>
								  <xsl:value-of select='COL25'/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select='concat(substring-before(COL25, " "), $varSuffix)'/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <!--<IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="substring-before(COL84, ' ') = 'Call' or substring-before(COL84, ' ') = 'Put'">
						  <xsl:value-of select='$varIDCO'/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select='""'/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>-->

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

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
              <xsl:when test="$varNetPosition &lt; 0">
                <Quantity>
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </Quantity>
              </xsl:when>
              <xsl:when test="$varNetPosition &gt; 0">
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

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="contains(COL84,'Call') or contains(COL84,'Put')">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Side>
				<xsl:choose>
					<xsl:when test="$Asset = 'Equity'">
						<xsl:choose>
							<xsl:when test="$varSide = 'Buy Long'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'Sell Long'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'Buy to Cover'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'Sell Short'">
								<xsl:value-of select="'Sell short'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$Asset='Option'">
						<xsl:choose>
							<xsl:when test="$varSide = 'Buy Long'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'Sell Long'">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'Buy to Cover'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'Sell Short'">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
				</xsl:choose>

			</Side>


            <!--<Commission>
              <xsl:choose>
                <xsl:when test="COL51 &gt; 0">
                  <xsl:value-of select="COL51"/>
                </xsl:when>
                <xsl:when test="COL51 &lt; 0">
                  <xsl:value-of select="COL51*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <StampDuty>
              <xsl:choose>
                <xsl:when test="COL52 &gt; 0">
                  <xsl:value-of select="COL52"/>
                </xsl:when>
                <xsl:when test="COL52 &lt; 0">
                  <xsl:value-of select="COL52*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>-->

			  <GrossNotionalValue>
				  <xsl:choose>
					  <xsl:when test="COL50 &gt; 0">
						  <xsl:value-of select="COL50"/>
					  </xsl:when>
					  <xsl:when test="COL50 &lt; 0">
						  <xsl:value-of select="COL50*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </GrossNotionalValue>

            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="COL58 &gt; 0">
                  <xsl:value-of select="COL58"/>
                </xsl:when>
                <xsl:when test="COL58 &lt; 0">
                  <xsl:value-of select="COL58*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test="COL70 &gt; 0">
						  <xsl:value-of select="COL70"/>
					  </xsl:when>
					  <xsl:when test="COL70 &lt; 0">
						  <xsl:value-of select="COL70*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>

            <CurrencySymbol>
              <xsl:value-of select="COL55"/>
            </CurrencySymbol>
			  <TradeDate>
				  <xsl:value-of select="normalize-space(COL36)"/>
			  </TradeDate>

			  <xsl:variable name="TotalCommissionandFees">
				  <xsl:value-of select="COL51 + COL52+ COL80"/>
			  </xsl:variable>

			  <TotalCommissionandFees>
				  <xsl:choose>
					  <xsl:when test="$TotalCommissionandFees &gt; 0">
						  <xsl:value-of select="$TotalCommissionandFees"/>
					  </xsl:when>
					  <xsl:when test="$TotalCommissionandFees &lt; 0">
						  <xsl:value-of select="$TotalCommissionandFees*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </TotalCommissionandFees>

						<SMRequest>
				  <xsl:value-of select ="'TRUE'"/>
			  </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public static string NowSwap(int year, int month, int date)
    {
    DateTime weekEnd= new DateTime(year, month, date);
    weekEnd = weekEnd.AddDays(1);
    while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
    {
    weekEnd = weekEnd.AddDays(1);
    }
    return weekEnd.ToString();
    }

  </msxsl:script>
	
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
	
	<xsl:template name="MonthInCode">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='Mar' or $Month='March'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="'12'"/>
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
				<xsl:when test="$Month='Jan'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='Feb'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='Mar' or $Month='March'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='Apr'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='May'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='Jun'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='Jul'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='Aug'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='Sep'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='Oct'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='Nov'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='Dec'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='Jan'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='Feb'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='Mar' or $Month='March'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='Apr'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='May'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='Jun'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='Jul'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='Aug'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='Sep'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='Oct'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='Nov'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='Dec'">
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
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
    </xsl:variable>
    <!--<xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring-before(substring-after(normalize-space(COL16),'/'),'/')"/>
    </xsl:variable>-->
    <xsl:variable name="ExpiryMonth">
		<!--<xsl:value-of select="substring-before(substring-after(normalize-space($Symbol),' '),' ')"/>-->
		<xsl:value-of select="substring-before(substring-after(normalize-space($Symbol),' '),' ')"/>
	</xsl:variable>
	<xsl:variable name="ExpiryYear">
		<!--<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL16),'/'),'/'),3,2)"/>-->
		<xsl:value-of select="'21'"/>
	</xsl:variable>

	<xsl:variable name="PutORCall">
		<!--<xsl:choose>
			<xsl:when test="contains(COL5,'/')">
          <xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),1,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),1,1)"/>
        </xsl:otherwise>
      </xsl:choose>-->
		<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),1,1)"/>
    </xsl:variable>
    
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),'##.00')"/>
    </xsl:variable>
	  <xsl:variable name="MonthCodVar">
		  <xsl:call-template name="MonthCode">
			  <xsl:with-param name="Month" select="$ExpiryMonth"/>
			  <xsl:with-param name="PutOrCall" select="$PutORCall"/>
		  </xsl:call-template>
	  </xsl:variable>
	  <xsl:variable name="VarMonthInCod">
		  <xsl:call-template name="MonthInCode">
			  <xsl:with-param name="Month" select="$ExpiryMonth"/>
		  </xsl:call-template>
	  </xsl:variable>

	  <xsl:variable name="ThirdFriday">
		  <xsl:choose>
			  <xsl:when test="number($VarMonthInCod) and number($ExpiryYear)">
				  <xsl:value-of select="my:Now(number(concat('20',$ExpiryYear)),number($VarMonthInCod))"/>
			  </xsl:when>
		  </xsl:choose>
	  </xsl:variable>
	  <!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>-->
	  <xsl:variable name="Day">
		  <xsl:value-of select="substring-before(substring-after($ThirdFriday,'/'),'/')"/>
	  </xsl:variable>
	  <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  



  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <AccountName>

				  <xsl:value-of select="'Events0919 GS PB 065029654'"/>

			  </AccountName>

            <!--<AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>-->

            <xsl:variable name = "varAsset" >
              <xsl:choose>
                <xsl:when test="COL2 ='clus' or COL2='ptus'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:when test="COL2='oaus'">
                  <xsl:value-of select="'FXForward'"/>
                </xsl:when>
                <xsl:when test="COL2='swus'">
                  <xsl:value-of select="'EquitySwap'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name ="varFXMonths">
              <xsl:value-of select="substring-before(substring-after(substring-after(COL5,' '),' '),'/')"/>
            </xsl:variable>

            <xsl:variable name="varFXDay">
              <xsl:value-of select="substring-before(substring-after(COL5,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varFXYear">
              <xsl:value-of select="substring-before(substring-after(substring-after(COL5,'/'),'/'),' ')"/>
            </xsl:variable>


            <xsl:variable name="varFXForward">
              <xsl:choose>
                <xsl:when test="substring-before(COL5,' ') ='EUR' or substring-before(COL5,' ')='GBP' or substring-before(COL5,' ')='NZD' or substring-before(COL5,' ')='AUD'">
                  <xsl:value-of select="concat(substring-before(COL5,' '),'/','USD',' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat('USD','/',substring-before(COL5,' '),' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSymbol">
				<xsl:choose>

					<xsl:when test="contains(COL3,'.')">
						<xsl:value-of select ="substring-before(translate(COL3,$vLowercaseChars_CONST,$vUppercaseChars_CONST),'.')"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="translate(COL3,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
					</xsl:otherwise>
				</xsl:choose>
              
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL5"/>
                    <xsl:with-param name="Suffix" select="''"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="$varAsset='FXForward'">
                  <xsl:value-of select="$varFXForward"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                

                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

			  <xsl:variable name="PB_CURRENCY_NAME">
				  <xsl:value-of select="''"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_CURRENCY_ID">
				  <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
			  </xsl:variable>

			  <CurrencyID>
				  <xsl:choose>
					  <xsl:when test="number($PRANA_CURRENCY_ID)">
						  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CurrencyID>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY"/>
            </PBSymbol>


            <NetPosition>
              <xsl:choose>
                <xsl:when  test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when  test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when  test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when  test="$varSide='by'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <!--<xsl:when  test="$varSide='BuyCover'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>-->
                    <xsl:when  test="$varSide='ss'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide='sl' or $varSide='cs'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>

                  <xsl:choose>
                    <xsl:when  test="$varSide='by'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <!--<xsl:when  test="$varSide='cs'">
                      <xsl:value-of select="''"/>
                    </xsl:when>-->
                    <xsl:when  test="$varSide='ss'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide='sl' or $varSide='cs'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
              
            </SideTagValue>
			
			  <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4 * COL6 * 0.0000221"/>
              </xsl:call-template>
            </xsl:variable>
			
			 <xsl:variable name="varSecFee1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL4 * COL6 * 100 * 0.0000221"/>
              </xsl:call-template>
            </xsl:variable>
			
			
				 <xsl:variable name="varSecFee2">
			 <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0 and COL1='sl' and $varAsset ='EquityOption'">
                  <xsl:value-of select="$varSecFee1"/>
                </xsl:when>
				 <xsl:when test="$varSecFee &gt; 0 and COL1='ss' and $varAsset ='EquityOption'">
                  <xsl:value-of select="$varSecFee1"/>
                </xsl:when>
				
				<xsl:when test="$varSecFee &lt; 0 and COL1='sl' and $varAsset ='EquityOption'">
                  <xsl:value-of select="$varSecFee1 * -1"/>
                </xsl:when>
				 <xsl:when test="$varSecFee &lt; 0 and COL1='ss' and $varAsset ='EquityOption'">
                  <xsl:value-of select="$varSecFee1 * -1"/>
                </xsl:when>
				 <xsl:when test="$varSecFee &gt; 0 and COL1='sl' and $varAsset ='Equity'">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
				 <xsl:when test="$varSecFee &gt; 0 and COL1='ss' and $varAsset ='Equity'">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
              <xsl:when test="$varSecFee &lt; 0 and COL1='sl' and $varAsset ='Equity'">
                  <xsl:value-of select="$varSecFee * -1"/>
                </xsl:when>
				 <xsl:when test="$varSecFee &lt; 0 and COL1='ss' and $varAsset ='Equity'">
                  <xsl:value-of select="$varSecFee * -1"/>
                </xsl:when>
              
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
			   </xsl:variable>
			
			 
			
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFee2 &gt; 0 and COL13='us'">
                  <xsl:value-of select="$varSecFee2"/>
                </xsl:when>
                <xsl:when test="$varSecFee2 &lt; 0 and COL13='us'">
                  <xsl:value-of select="$varSecFee2"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>
			
			<xsl:variable name="ORFFee2">
              <xsl:value-of select="0.0357 * COL4"/>
            </xsl:variable>


			  <OrfFee>
              <xsl:choose>
                <xsl:when test="$ORFFee2 &gt; 0 and   $varAsset ='EquityOption' and COL13='us'">
                  <xsl:value-of select="$ORFFee2"/>
                </xsl:when>
                <xsl:when test="$ORFFee2 &lt; 0 and   $varAsset ='EquityOption' and COL13='us'">
                  <xsl:value-of select="$ORFFee2 * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>




            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL17,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL17,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL17,1,4)"/>
            </xsl:variable>

            <xsl:variable name="varDateTime">
              <xsl:value-of select="COL9"/>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varDateTime"/>
            </PositionStartDate>

            <!--<xsl:variable name="varOTransDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="''"/>
              </xsl:call-template>
            </xsl:variable>-->

            <xsl:variable name="varOrigTransDate">
              <xsl:value-of select="$varDateTime"/>
            </xsl:variable>

            <xsl:if test="contains(COL2,'swus')">

              <IsSwapped>
                <xsl:value-of select ="1"/>
              </IsSwapped>

              <SwapDescription>
                <xsl:value-of select ="'SWAP'"/>
              </SwapDescription>

              <DayCount>
                <xsl:value-of select ="365"/>
              </DayCount>

              <ResetFrequency>
                <xsl:value-of select ="'Monthly'"/>
              </ResetFrequency>

              <OrigTransDate>
                <xsl:value-of select ="$varOrigTransDate"/>
              </OrigTransDate>
              <xsl:variable name="varSYear">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'/'),'/')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'-'),'-')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Day">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'/'),'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'/'),'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'/'),'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'-'),'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'-'),'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'-'),'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Month">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name="SettleDate">
                <xsl:value-of select='my:NowSwap(number($varSYear),number($Month),number($Day))'/>
              </xsl:variable>

              <FirstResetDate>
                <xsl:value-of select ="$SettleDate"/>
              </FirstResetDate>
            </xsl:if>
         
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>



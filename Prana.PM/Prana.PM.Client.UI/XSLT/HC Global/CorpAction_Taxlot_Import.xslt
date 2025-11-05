<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
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
  

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL11)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and COL2!='*'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Triad'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						
						<xsl:variable name="Asset">
							<xsl:choose>

								<xsl:when test="contains(COL5,'O:')">
									<xsl:value-of select="'Option'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<Symbol>


							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*' or $Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL8"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>

								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>
            
						<SideTagValue>

							<xsl:choose>
								<xsl:when test="$Asset='Option'">
									<xsl:choose>
										<xsl:when test="(COL10='Long' and COL22='old')">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="(COL10='Short' and COL22='old')">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="(COL10='Long' and COL22='new')">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="(COL10='Short' and COL22s='new')">
											<xsl:value-of select="'C'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
                
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="(COL10='Long' and COL22='old')">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="(COL10='Short' and COL22='old')">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="(COL10='Long' and COL22='new')">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="(COL10='Short' and COL22='new')">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

            <xsl:variable name="varDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="COL2"/>
              </xsl:call-template>
            </xsl:variable>
            
						<xsl:if test="contains($Symbol,'SWAP')">

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
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL1"/>
                </xsl:call-template>
              </OrigTransDate>

              <xsl:variable name="varODate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring-before(substring-after($varDate,'/'),'/')"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varOYear">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring-after(substring-after($varDate,'/'),'/')"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varOMonth">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring-before($varDate,'/')"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="SettleDate">
                <xsl:value-of select='my:NowSwap(number($varOYear),number($varOMonth),number($varODate))'/>
              </xsl:variable>

              <FirstResetDate>
                <xsl:value-of select ="$SettleDate"/>
              </FirstResetDate>
            </xsl:if>

						<xsl:variable name="PickDate">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<OriginalPurchaseDate>
							<xsl:choose>
								<xsl:when test="(COL23='new')">
									<xsl:value-of select="$varDate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</OriginalPurchaseDate>
						
						<PositionSettlementDate>
							<xsl:value-of select="'08/26/2021'"/>
						</PositionSettlementDate>
						
						<CounterPartyID>
							<xsl:value-of select="'59'"/>
						</CounterPartyID>
						
						<PBAssetType>
							<xsl:value-of select="$Asset"/>
						</PBAssetType>
						
						<CurrencyID>
							<xsl:choose>
							<xsl:when test="(COL10='USD')">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>
							</xsl:choose>
						</CurrencyID>
						

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
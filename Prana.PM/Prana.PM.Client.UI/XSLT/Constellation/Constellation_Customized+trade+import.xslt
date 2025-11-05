<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

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

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'$',''))"/>
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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and contains(COL2,'Trades')"/>

					
				<PositionMaster>

					<!--<FundName>
            <xsl:value-of select="''"/>
          </FundName>-->

            		<Symbol>
						<xsl:value-of select="COL6"/>
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL26"/>
					</PBSymbol>


					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="COL26"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>

							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>
					<xsl:variable name="Asset">
						<xsl:choose>

							<xsl:when test="contains(COL6,'SWAP')">
								<xsl:value-of select="'EquitySwap'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NetPosition>
						<xsl:choose>
							<xsl:when test="$NetPosition&gt; 0">
								<xsl:value-of select="$NetPosition"/>
							</xsl:when>
							<xsl:when test="$NetPosition&lt; 0">
								<xsl:value-of select="$NetPosition* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>


					<xsl:choose>
						<xsl:when test ="COL9='BL'and COL45= 'Options'">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL9='BS'and COL45= 'Options'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL9='SS'and COL45= 'Options'">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL9='SL'and COL45= 'Options'">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when >
							<xsl:when test ="COL9='SL' ">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL9='SS' ">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL9='BS' ">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL9='BL' ">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<!--<xsl:when test ="COL5 &lt; 0 ">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >-->
						
						<!--<xsl:when test ="COL7='Buy to Close'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="(COL7='Sell short' or COL7='Sell Short') and COL7= 'Equity'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Sell to Open'">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Buy to Open'">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Sell to Close'">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when >-->

						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:variable name="CostBasis" select="COL15"/>

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

					<xsl:variable name="Commission" select="COL28"/>
					<Commission>
						<xsl:choose>
							<xsl:when test="$Commission &gt; 0">
								<xsl:value-of select="$Commission"/>

							</xsl:when>
							<xsl:when test="$Commission &lt; 0">
								<xsl:value-of select="$Commission * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</Commission>

					<xsl:variable name="SecFee" select="COL30"/>
					<SecFee>
						<xsl:choose>
							<xsl:when test="$SecFee &gt; 0">
								<xsl:value-of select="$SecFee"/>

							</xsl:when>
							<xsl:when test="$SecFee &lt; 0">
								<xsl:value-of select="$SecFee * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</SecFee>
					
					<xsl:variable name="OrfFee" select="COL67"/>
					<OrfFee>
						<xsl:choose>
							<xsl:when test="$OrfFee &gt; 0">
								<xsl:value-of select="$OrfFee"/>

							</xsl:when>
							<xsl:when test="$OrfFee &lt; 0">
								<xsl:value-of select="$OrfFee * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</OrfFee>


					<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="normalize-space(COL23)"/>
					</xsl:variable>

					<xsl:variable name="PRANA_BROKER_ID">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='CON']/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
					</xsl:variable>

					<CounterPartyID>
						<xsl:choose>
							<xsl:when test="number($PRANA_BROKER_ID)">
								<xsl:value-of select="$PRANA_BROKER_ID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CounterPartyID>
					
					<PositionStartDate>
						<xsl:value-of select="COL11"/>
					</PositionStartDate>

					<xsl:if test="$Asset='EquitySwap'">

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
							<xsl:value-of select ="COL11"/>
						</OrigTransDate>

						<xsl:variable name="varPrevMonth">
							<xsl:choose>
								<xsl:when test ="number(substring-before(COL11,'/')) = 1">
									<xsl:value-of select ="12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-before(COL11,'/'))-1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varYear">
							<xsl:choose>
								<xsl:when test ="number(substring-before(COL11,'/')) = 1">
									<xsl:value-of select ="number(substring-after(substring-after(COL11,'/'),'/')) -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-after(substring-after(COL11,'/'),'/'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<FirstResetDate>
							<xsl:value-of select ="concat($varPrevMonth,'/28/',$varYear)"/>
						</FirstResetDate>
					</xsl:if>

					<xsl:variable name="varFXRate">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL17"/>
						</xsl:call-template>
					</xsl:variable>		
					

					<FXRate>
						<xsl:choose>
							<xsl:when test="COL32='EUR' ">
								<xsl:value-of select="$varFXRate"/>
							</xsl:when>							
							
							<xsl:otherwise>
								<xsl:value-of select="1 div $varFXRate"/>
							</xsl:otherwise>

						</xsl:choose>
					</FXRate>
					<SettlCurrFxRate>
						<xsl:choose>
							<xsl:when test="COL32='EUR' ">
								<xsl:value-of select="$varFXRate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="1 div $varFXRate"/>
							</xsl:otherwise>

						</xsl:choose>
					</SettlCurrFxRate>

					<SettlCurrencyName>
						<xsl:value-of select="COL1"/>
					</SettlCurrencyName>
					

					<!--<OriginalPurchaseDate>
						<xsl:value-of select="COL11"/>
					</OriginalPurchaseDate>-->

					

				
			</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>

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

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:param name="varPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth='01' and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='01' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL10)">
					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<PositionStartDate>
							<xsl:value-of select ="COL1"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="COL2"/>
						</PositionSettlementDate>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number($varAvgPrice)" >
									<xsl:value-of select ="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BOFA']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name ="varUnderlyingSymbol">
							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select ="substring-before(normalize-space(COL18),' ')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExpireDate">

							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select="substring(substring-after(normalize-space(COL18),' '),1,6)"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select="substring($varExpireDate,1,2)"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varMonth">

							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select="substring($varExpireDate,3,2)"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varExDay">
							<xsl:value-of select="substring($varExpireDate,5,2)"/>
						</xsl:variable>


						<xsl:variable name="varPutCall">

							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select ="substring(substring-after(normalize-space(COL18),' '),7,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varStrikePrice">
							<xsl:choose>
								<xsl:when test ="COL18 !=''">
									<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space(COL7),' '),' '),' '),' ')"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name ="varMonthCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonth"/>
								<xsl:with-param name="varPutCall" select="$varPutCall"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varDays">
							<xsl:choose>
								<xsl:when test="substring($varExDay,1,1)='0'">
									<xsl:value-of select="substring($varExDay,2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varExDay"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varThirdFriday">
							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select='my:Now(number(concat(20,$varYear)),number($varMonth))'/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test ="COL18 !='' and not(contains(COL7,'TREASURY BILL'))">
									<xsl:value-of select="$varOptionSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL6)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varSide">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="varStatus">
							<xsl:value-of select="COL16"/>
						</xsl:variable>
						
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varStatus='O'">
									<xsl:choose>
										<xsl:when test ="$varSide='S'">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:when test ="$varSide='B'">
											<xsl:value-of select ="'A'"/>
										</xsl:when>										
									</xsl:choose>
								</xsl:when>

								<xsl:when test="$varStatus='X'">
									<xsl:choose>
										<xsl:when test ="$varSide='S'">
											<xsl:value-of select ="'D'"/>
										</xsl:when>
										<xsl:when test ="$varSide='B'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$varSide='S'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="$varSide='B'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
							
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="COL7"/>
						</PBSymbol>

						<xsl:variable name="varCommision">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($varCommision) &lt; 0'>
									<xsl:value-of select ='$varCommision*-1'/>
								</xsl:when>

								<xsl:when test ='number($varCommision) &gt; 0'>
									<xsl:value-of select ='$varCommision'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varFees">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($varFees) &lt; 0'>
									<xsl:value-of select ='$varFees*-1'/>
								</xsl:when>

								<xsl:when test ='number($varFees) &gt; 0'>
									<xsl:value-of select ='$varFees'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<!--<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BOFA']/BrokerData[@PBBroker=$PB_BROKER_NAME]/@PranaBrokerID"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$PRANA_BROKER_NAME!=''">
									<xsl:value-of select="$PRANA_BROKER_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

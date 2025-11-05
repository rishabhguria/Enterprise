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

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL8)">
					<PositionMaster>						

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Touradji']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						

						<xsl:variable name ="NetPosition">

							<xsl:value-of select ="number(COL8)"/>

						</xsl:variable>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="number(COL9)"/>
						</xsl:variable>

						<xsl:variable name="varFutureYear">
							<xsl:value-of select="substring(substring-after(COL2,' '),6)"/>
						</xsl:variable>

						<xsl:variable name="varFutureMonth">
							<xsl:value-of select="substring(substring-after(COL2,' '),1,2)"/>
						</xsl:variable>

						<xsl:variable name="varFuturedate">
							<xsl:value-of select="substring(substring-after(COL2,' '),3,2)"/>
						</xsl:variable>
						
						<xsl:variable name="varFutureMonthCode">
							<xsl:choose>
								<xsl:when test ="$varFutureMonth='01'">
									<xsl:value-of select ="'F'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='02' ">
									<xsl:value-of select ="'G'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='03'">
									<xsl:value-of select ="'H'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='04' ">
									<xsl:value-of select ="'J'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='05' ">
									<xsl:value-of select ="'K'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='06'">
									<xsl:value-of select ="'M'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='07' ">
									<xsl:value-of select ="'N'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='08' ">
									<xsl:value-of select ="'Q'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='09' ">
									<xsl:value-of select ="'U'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='10' ">
									<xsl:value-of select ="'V'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='11'">
									<xsl:value-of select ="'X'"/>
								</xsl:when>
								<xsl:when test ="$varFutureMonth='12'">
									<xsl:value-of select ="'Z'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:variable name="varStrikePrice">
							<xsl:choose>
								<xsl:when test="COL1='Future Options' and normalize-space(substring-after(substring-after(COL2,' '),' ')) != ''">
									<xsl:value-of select="substring-before(substring-after(COL2,' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(COL2,' ')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->
						
						<PBAssetType>
							<xsl:value-of select ="COL1"/>
						</PBAssetType>

						<xsl:variable name = "PB_ROOT_SYMBOL">
							<xsl:choose>
								<xsl:when test="COL1='Future Options'">
									<xsl:value-of select="normalize-space(substring(COL2,1,2))"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="PRANA_MULTIPLIER">
							<xsl:value-of select ="document('../ReconMappingXml/PriceMulMapping.xml')/PriceMulMapping/PB[@Name='JPM']/MultiplierData[@PranaRoot=$PB_ROOT_SYMBOL]/@Multiplier"/>
						</xsl:variable>

						<xsl:variable name="varStrikePrice">
							<xsl:choose>
								<xsl:when test="COL1='Future Options'">
									<xsl:choose>
										<xsl:when test ="contains(COL2,'CURNCY') != false">
											<xsl:value-of select ="normalize-space(substring-after(substring-before(COL2, 'CURNCY'),' '))"/>
										</xsl:when>
										
										<xsl:when test ="contains(COL2,'INDEX') != false">
											<xsl:value-of select ="normalize-space(substring-after(substring-before(COL2, 'INDEX'),' '))"/>
										</xsl:when>	
										
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test ="number(substring-after(COL2,' '))">
													<xsl:value-of select ="substring-after(COL2,' ')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select ="substring-after(substring-after(COL2,' '),' ')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>									
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(COL2,' ')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<PBSymbol>
							<xsl:value-of select ="concat('PB_ROOT_SYMBOL',' ' ,$PRANA_MULTIPLIER)"/>
						</PBSymbol>

						

						<Symbol>
							<xsl:choose>
								<xsl:when test="COL1='Fixed Income'">
									<xsl:value-of select="COL2"/>
								</xsl:when>

								<!--Future -->
								<xsl:when test="COL1='Future'">
									<xsl:choose>
										<xsl:when test="contains(COL3,'LME')=false">
											<xsl:value-of select="normalize-space(concat(substring(COL2,1,2),' ',substring(COL2,3,2)))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(substring(substring-before(COL2,' '),3),' ',$varFutureYear,$varFutureMonthCode,$varFuturedate,'-LME')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>




								<!--LME Future Options-->
								<xsl:when test="COL1='Future Options'">
									<xsl:choose>
										<xsl:when test="$PRANA_MULTIPLIER!=''">
											<xsl:value-of select="concat(normalize-space(concat(substring(COL2,1,2),' ',substring(COL2,3,2))),substring(COL2,5,1),$varStrikePrice*$PRANA_MULTIPLIER)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(normalize-space(concat(substring(COL2,1,2),' ',substring(COL2,3,2))),substring(COL2,5,1),$varStrikePrice)"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>				


							</xsl:choose>

						</Symbol>

						

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
							<xsl:value-of select ="COL4"/>
						</PositionStartDate>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="'2'"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
					
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
